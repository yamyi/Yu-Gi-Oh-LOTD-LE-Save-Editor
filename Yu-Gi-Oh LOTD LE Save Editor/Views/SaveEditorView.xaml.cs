using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// Full save-data editor, ported from the WinForms build's SaveEditorPage -
/// Duel Points / Campaign / Unlocks / Avatars / Cards, each a section Grid
/// declared in SaveEditorView.xaml and toggled by ShowSection (no runtime
/// control creation - the tab row just swaps Style/Visibility on
/// already-declared elements). Checklists (Shop Packs/Tutorials/Avatars) and
/// Campaign duel rows are ObservableCollection&lt;T&gt; data bound to
/// DataTemplates in the XAML, same pattern as every other view here.
/// Every mutation snapshots the undo stack first and raises SaveDataChanged
/// afterwards, exactly like DeckSlotsView/DeckEditorView; _suppressEvents
/// guards every place this view writes UI state back onto itself so a
/// sync-from-save doesn't get misread as a user edit.
/// </summary>
public partial class SaveEditorView : UserControl
{
    public event EventHandler? SaveDataChanged;

    private bool _suppressEvents;
    private int _selectedSeriesIndex;
    private string _currentTabKey = "DuelPoints";

    private byte[]? Bytes => AppContext.State?.SaveBytes;
    private LotdSaveVersion Version => AppContext.State?.Version ?? LotdSaveVersion.LinkEvolution;
    private bool HasSave => Bytes is { Length: > 0 };

    private Dictionary<string, (Button Button, Grid Section)> _tabs = new();
    private Dictionary<LotdDuelSeries, Button> _seriesButtons = new();

    public ObservableCollection<DuelRowViewModel> DuelRows { get; } = new();
    public ObservableCollection<CheckableItem> ShopPackItems { get; } = new();
    public ObservableCollection<CheckableItem> TutorialItems { get; } = new();
    public ObservableCollection<CheckableItem> AvatarItems { get; } = new();
    public ObservableCollection<StatRowViewModel> StatItems { get; } = new();
    public ObservableCollection<CardSearchResultViewModel> CardSearchResults { get; } = new();

    private const int CardSearchResultCap = 40;
    private int? _selectedCardLotdId;

    private static readonly string[] DuelStateOptions =
        { "Locked", "Available", "AvailableAttempted", "Complete", "AvailableAlt" };

    /// <summary>Bound via ElementName=Root from DuelRowTemplate's two
    /// ComboBoxes - a plain static option list, same convention as every
    /// other cross-element binding in this app.</summary>
    public string[] DuelStateNames => DuelStateOptions;

    public SaveEditorView()
    {
        InitializeComponent();

        _tabs = new Dictionary<string, (Button, Grid)>
        {
            ["DuelPoints"] = (TabDuelPointsButton, DuelPointsSection),
            ["Campaign"] = (TabCampaignButton, CampaignSection),
            ["Unlocks"] = (TabUnlocksButton, UnlocksSection),
            ["Avatars"] = (TabAvatarsButton, AvatarsSection),
            ["Stats"] = (TabStatsButton, StatsSection),
            ["Cards"] = (TabCardsButton, CardsSection),
        };

        _seriesButtons = new Dictionary<LotdDuelSeries, Button>
        {
            [LotdDuelSeries.YuGiOh] = SeriesYuGiOhButton,
            [LotdDuelSeries.YuGiOhGX] = SeriesGXButton,
            [LotdDuelSeries.YuGiOh5D] = Series5DButton,
            [LotdDuelSeries.YuGiOhZEXAL] = SeriesZEXALButton,
            [LotdDuelSeries.YuGiOhARCV] = SeriesARCVButton,
            [LotdDuelSeries.YuGiOhVRAINS] = SeriesVRAINSButton,
        };

        DuelRowsList.ItemsSource = DuelRows;
        ShopPacksList.ItemsSource = ShopPackItems;
        TutorialsList.ItemsSource = TutorialItems;
        AvatarsList.ItemsSource = AvatarItems;
        StatsList.ItemsSource = StatItems;
        CardSearchResultsList.ItemsSource = CardSearchResults;

        RefreshFromState();
    }

    /// <summary>Rebuilds every tab's displayed values from the currently
    /// loaded save. Call after a save is (re)loaded and after undo/redo
    /// swaps AppContext.State.SaveBytes - wired from MainWindow the same way
    /// DeckSlotsPage/DeckEditorPage's own refresh hooks are.</summary>
    public void RefreshFromState()
    {
        RefreshDuelPointsTab();
        RefreshCampaignTab();
        RefreshUnlocksTab();
        RefreshAvatarsTab();
        RefreshStatsTab();
        RefreshCardsTab();
        ApplyTabVisuals();
    }

    private void RaiseSaveDataChanged() => SaveDataChanged?.Invoke(this, EventArgs.Empty);

    // ── Tab switching ────────────────────────────────────────────────────────

    private void TabButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is string key && _tabs.ContainsKey(key))
        {
            _currentTabKey = key;
            ApplyTabVisuals();
        }
    }

    private void ApplyTabVisuals()
    {
        var active = (Style)FindResource("SaveEditorTabActiveStyle");
        var inactive = (Style)FindResource("SaveEditorTabInactiveStyle");

        NoSavePlaceholder.Visibility = HasSave ? Visibility.Collapsed : Visibility.Visible;

        foreach (var (name, pair) in _tabs)
        {
            bool isActive = name == _currentTabKey;
            pair.Button.Style = isActive ? active : inactive;
            pair.Section.Visibility = isActive && HasSave ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    // Duel Points
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshDuelPointsTab()
    {
        DuelPointsBox.IsEnabled = HasSave;
        if (!HasSave)
        {
            DuelPointsCurrentText.Text = "Current: —";
            return;
        }

        long current = MiscSaveLayout.GetDuelPoints(Bytes!, Version);
        DuelPointsCurrentText.Text = $"Current: {current:N0} DP";
        DuelPointsBox.Text = current.ToString();
    }

    private void SetDuelPoints_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        if (!long.TryParse(DuelPointsBox.Text, out long value)) return;
        value = Math.Clamp(value, 0L, 999_999_999L);

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetDuelPoints(Bytes!, Version, value);
        RaiseSaveDataChanged();
        RefreshDuelPointsTab();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Campaign
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>Confirmed-real duel ranges, per series, within the fixed
    /// 50-slot save array - 0-based index of the first real duel, and how
    /// many real duels follow it. Everything outside that range is unused
    /// padding and gets hidden from the list entirely. Series not listed
    /// here fall back to showing the full unfiltered 50. Ported verbatim
    /// from the WinForms build's SaveEditorPage.KnownRealDuelRange.</summary>
    private static readonly Dictionary<LotdDuelSeries, (int RealStart, int RealCount)> KnownRealDuelRange = new()
    {
        { LotdDuelSeries.YuGiOh, (1, 32) },
        { LotdDuelSeries.YuGiOhGX, (1, 32) },
        { LotdDuelSeries.YuGiOh5D, (1, 32) },
        { LotdDuelSeries.YuGiOhZEXAL, (1, 26) },
        { LotdDuelSeries.YuGiOhARCV, (1, 33) },
        { LotdDuelSeries.YuGiOhVRAINS, (1, 28) },
    };

    private void RefreshCampaignTab()
    {
        if (HasSave)
        {
            var series = CampaignSaveLayout.GetSeries(Version);
            if (_selectedSeriesIndex >= series.Length) _selectedSeriesIndex = 0;
        }

        ApplySeriesButtonVisuals();
        RebuildDuelGrid();
    }

    /// <summary>Click a series' own logo to select it (no ComboBox) - swaps
    /// every logo button's Style between SeriesLogoActiveStyle/InactiveStyle
    /// depending on whether it matches the currently-selected series, and
    /// hides the VRAINS button entirely for the older "Lotd" save format
    /// (GetSeries only returns 5 series for that version - no VRAINS
    /// campaign exists in that save format at all).</summary>
    private void ApplySeriesButtonVisuals()
    {
        var active = (Style)FindResource("SeriesLogoActiveStyle");
        var inactive = (Style)FindResource("SeriesLogoInactiveStyle");
        var series = HasSave ? CampaignSaveLayout.GetSeries(Version) : Array.Empty<LotdDuelSeries>();
        LotdDuelSeries? currentSeries = _selectedSeriesIndex >= 0 && _selectedSeriesIndex < series.Length
            ? series[_selectedSeriesIndex]
            : null;

        foreach (var (seriesValue, button) in _seriesButtons)
        {
            button.Visibility = Array.IndexOf(series, seriesValue) >= 0 ? Visibility.Visible : Visibility.Collapsed;
            button.Style = seriesValue == currentSeries ? active : inactive;
        }
    }

    private void SeriesLogoButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not string tag || !Enum.TryParse<LotdDuelSeries>(tag, out var series)) return;

        var allSeries = CampaignSaveLayout.GetSeries(Version);
        int idx = Array.IndexOf(allSeries, series);
        if (idx < 0) return;

        _selectedSeriesIndex = idx;
        ApplySeriesButtonVisuals();
        RebuildDuelGrid();
    }

    /// <summary>Maps a raw duel-state value to one of the 5 named states -
    /// some never-touched slots hold raw values outside the 5-member enum,
    /// which would otherwise not match any of DuelStateNames' entries.</summary>
    private static string DisplayState(LotdCampaignDuelState state)
    {
        int raw = (int)state;
        if (raw < 0) return LotdCampaignDuelState.Locked.ToString();
        if (raw > (int)LotdCampaignDuelState.AvailableAlt) return LotdCampaignDuelState.AvailableAlt.ToString();
        return state.ToString();
    }

    private void RebuildDuelGrid()
    {
        _suppressEvents = true;
        try
        {
            DuelRows.Clear();
            if (!HasSave) return;

            var series = CampaignSaveLayout.GetSeries(Version);
            LotdDuelSeries? currentSeries = _selectedSeriesIndex >= 0 && _selectedSeriesIndex < series.Length
                ? series[_selectedSeriesIndex]
                : null;

            int realStart = 0;
            int realCount = CampaignSaveLayout.DuelsPerSeries;
            if (currentSeries.HasValue && KnownRealDuelRange.TryGetValue(currentSeries.Value, out var range))
            {
                realStart = range.RealStart;
                realCount = range.RealCount;
            }

            for (int i = 0; i < realCount; i++)
            {
                int d = realStart + i;
                var state = CampaignSaveLayout.GetState(Bytes!, Version, _selectedSeriesIndex, d);
                var reverse = CampaignSaveLayout.GetReverseState(Bytes!, Version, _selectedSeriesIndex, d);

                var names = currentSeries.HasValue ? CampaignDuelNames.Get(currentSeries.Value, i + 1) : null;
                string title = names?.Title ?? $"Duel {i + 1}";
                string matchup = names?.Matchup ?? string.Empty;
                byte ownerA = names?.OwnerA ?? 0;
                byte ownerB = names?.OwnerB ?? 0;

                DuelRows.Add(new DuelRowViewModel(i + 1, d, title, matchup, ownerA, ownerB, DisplayState(state), DisplayState(reverse)));
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private void DuelForward_SelectionChanged(object sender, SelectionChangedEventArgs e) => DuelStateChanged(sender, isForward: true);
    private void DuelReverse_SelectionChanged(object sender, SelectionChangedEventArgs e) => DuelStateChanged(sender, isForward: false);

    private void DuelStateChanged(object sender, bool isForward)
    {
        if (_suppressEvents || !HasSave) return;
        if (sender is not ComboBox combo || combo.DataContext is not DuelRowViewModel row) return;
        if (combo.SelectedItem is not string text || !Enum.TryParse<LotdCampaignDuelState>(text, out var state)) return;

        // The real first duel of a series (display row 1, array index 1) must
        // stay at least Available or the whole series becomes unclickable in
        // game - matches the bulk buttons' own guard in CampaignSaveLayout.SetSeries.
        if (isForward && row.DisplayNumber == 1 && state == LotdCampaignDuelState.Locked)
        {
            state = LotdCampaignDuelState.Available;
            _suppressEvents = true;
            combo.SelectedItem = DisplayState(state);
            _suppressEvents = false;
        }

        AppContext.Undo.Snapshot();
        if (isForward)
            CampaignSaveLayout.SetState(Bytes!, Version, _selectedSeriesIndex, row.DuelIndex, state);
        else
            CampaignSaveLayout.SetReverseState(Bytes!, Version, _selectedSeriesIndex, row.DuelIndex, state);
        RaiseSaveDataChanged();
    }

    private void UnlockSeries_Click(object sender, RoutedEventArgs e) => BulkSetSeries(LotdCampaignDuelState.Available);
    private void CompleteSeries_Click(object sender, RoutedEventArgs e) => BulkSetSeries(LotdCampaignDuelState.Complete);
    private void ResetSeries_Click(object sender, RoutedEventArgs e) => BulkSetSeries(LotdCampaignDuelState.Locked);

    private void BulkSetSeries(LotdCampaignDuelState state)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        // Apply to both directions - forward and reverse duels are tracked
        // separately, so a bulk action that only touched forward would leave
        // every Reverse cell exactly where it was.
        CampaignSaveLayout.SetSeries(Bytes!, Version, _selectedSeriesIndex, state, state);
        RaiseSaveDataChanged();
        RebuildDuelGrid();
    }

    private void UnlockAllCampaign_Click(object sender, RoutedEventArgs e) => BulkSetAll(LotdCampaignDuelState.Available);
    private void CompleteAllCampaign_Click(object sender, RoutedEventArgs e) => BulkSetAll(LotdCampaignDuelState.Complete);
    private void ResetAllCampaign_Click(object sender, RoutedEventArgs e) => BulkSetAll(LotdCampaignDuelState.Locked);

    private void BulkSetAll(LotdCampaignDuelState state)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        CampaignSaveLayout.SetAllSeries(Bytes!, Version, state, state);
        RaiseSaveDataChanged();
        RebuildDuelGrid();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Unlocks (shop packs, battle packs, content flags, tutorials, challenges/recipes)
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>Epic Dawn (ShopPacks bit 31) and Blue Angel/Soulburner/Varis/Ai
    /// (BattlePacksFlag bits 0/3/4/5) are all real duelist/pack unlocks sold
    /// alongside these ones in-game, but their actual save-file location
    /// isn't in this array - see UnlockedShopPacks' and UnlockedBattlePacks'
    /// doc comments for how each was found. They're surfaced below/in the
    /// Battle Packs section wired to their real fields instead, so the UI
    /// matches how players actually think about them.</summary>
    private readonly record struct ShopPackEntry(uint Flag, string Label);

    private static readonly ShopPackEntry[] ShopPackLabels =
    {
        new((uint)UnlockedShopPacks.GrandpaMuto, "Grandpa Muto"),
        new((uint)UnlockedShopPacks.MaiValentine, "Mai Valentine"),
        new((uint)UnlockedShopPacks.Bakura, "Bakura"),
        new((uint)UnlockedShopPacks.JoeyWheeler, "Joey Wheeler"),
        new((uint)UnlockedShopPacks.SetoKaiba, "Seto Kaiba"),
        new((uint)UnlockedShopPacks.Yugi, "Yugi"),
        new((uint)UnlockedShopPacks.AlexisRhodes, "Alexis Rhodes"),
        new((uint)UnlockedShopPacks.BastionMisawa, "Bastion Misawa"),
        new((uint)UnlockedShopPacks.ChazzPrinceton, "Chazz Princeton"),
        new((uint)UnlockedShopPacks.SyrusTruesdale, "Syrus Truesdale"),
        new((uint)UnlockedShopPacks.JesseAnderson, "Jesse Anderson"),
        new((uint)UnlockedShopPacks.JadenYuki, "Jaden Yuki"),
        new((uint)UnlockedShopPacks.TetsuTrudge, "Tetsu Trudge"),
        new((uint)UnlockedShopPacks.LeoLuna, "Leo/Luna"),
        new((uint)UnlockedShopPacks.AkizaIzinski, "Akiza Izinski"),
        new((uint)UnlockedShopPacks.JackAtlas, "Jack Atlas"),
        new((uint)UnlockedShopPacks.Crow, "Crow"),
        new((uint)UnlockedShopPacks.YuseiFudo, "Yusei Fudo"),
        new((uint)UnlockedShopPacks.CathyKatherine, "Cathy Katherine"),
        new((uint)UnlockedShopPacks.Quinton, "Quinton"),
        new((uint)UnlockedShopPacks.KiteTenjo, "Kite Tenjo"),
        new((uint)UnlockedShopPacks.Shark, "Shark"),
        new((uint)UnlockedShopPacks.YumaTsukumo, "Yuma Tsukumo"),
        new((uint)UnlockedShopPacks.GongStrong, "Gong Strong"),
        new((uint)UnlockedShopPacks.ZuzuBoyle, "Zuzu Boyle"),
        new((uint)UnlockedShopPacks.Shay, "Shay"),
        new((uint)UnlockedShopPacks.DeclanAkaba, "Declan Akaba"),
        new((uint)UnlockedShopPacks.YuyaSakaki, "Yuya Sakaki"),
        new((uint)UnlockedShopPacks.Playmaker, "Playmaker"),
        // BlueAngel/Soulburner/Varis/Ai (BattlePacksFlag bits, not ShopPacks)
        // are added separately below via BattlePackShopEntries.
        // EpicDawn (ShopPacks bit 31) deliberately not listed here - see doc
        // comment above. It's a real shop-pack bit, but shown under Battle
        // Packs, where players expect it.
    };

    /// <summary>Shop duelists whose real flag lives in BattlePacksFlag, not
    /// ShopPacks (see UnlockedBattlePacks' doc comment). Each entry's Kind
    /// doubles as its CheckableItem.Kind so CheckableItem_Changed and
    /// BulkShopPacks can look up the right flag generically instead of one
    /// hardcoded case per duelist. All 4 are CONFIRMED (BlueAngel via a real
    /// before/after save diff, Soulburner/Varis/Ai via live in-game testing
    /// after being wired up as a numbering hypothesis - see the enum's doc
    /// comment).</summary>
    private static readonly (UnlockedBattlePacks Flag, string Kind, string Label)[] BattlePackShopEntries =
    {
        (UnlockedBattlePacks.BlueAngel, "BlueAngel", "Blue Angel"),
        (UnlockedBattlePacks.Soulburner, "Soulburner", "Soulburner"),
        (UnlockedBattlePacks.Varis, "Varis", "Varis"),
        (UnlockedBattlePacks.Ai, "Ai", "Ai"),
    };

    private void RefreshUnlocksTab()
    {
        _suppressEvents = true;
        try
        {
            ShopPackItems.Clear();
            TutorialItems.Clear();

            if (!HasSave)
            {
                ContentChallengesCheck.IsChecked = false;
                ContentBattlePackCheck.IsChecked = false;
                ContentCardShopCheck.IsChecked = false;
                BpEpicDawnCheck.IsChecked = false;
                BpWotgCheck.IsChecked = false;
                BpWotgRound2Check.IsChecked = false;
                return;
            }

            var content = MiscSaveLayout.GetUnlockedContent(Bytes!, Version);
            ContentChallengesCheck.IsChecked = (content & UnlockedContent.DuelistChallenges) != 0;
            ContentBattlePackCheck.IsChecked = (content & UnlockedContent.BattlePack) != 0;
            ContentCardShopCheck.IsChecked = (content & UnlockedContent.CardShop) != 0;

            var bp = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
            BpWotgCheck.IsChecked = (bp & UnlockedBattlePacks.WarOfTheGiants) != 0;
            BpWotgRound2Check.IsChecked = (bp & UnlockedBattlePacks.WarOfTheGiantsRound2) != 0;

            var shop = (uint)MiscSaveLayout.GetShopPacks(Bytes!, Version);
            // Epic Dawn's real flag lives in ShopPacks (bit 31), not
            // BattlePacksFlag - confirmed via a real before/after save diff,
            // see UnlockedShopPacks' doc comment.
            BpEpicDawnCheck.IsChecked = (shop & (uint)UnlockedShopPacks.EpicDawn) != 0;
            for (int i = 0; i < ShopPackLabels.Length; i++)
            {
                var entry = ShopPackLabels[i];
                bool isChecked = (shop & entry.Flag) != 0;
                ShopPackItems.Add(new CheckableItem(entry.Label, i, "ShopPack", isChecked));
            }
            // Blue Angel/Soulburner/Varis/Ai are sold as normal duelist shop
            // packs in-game, but their real flags live in BattlePacksFlag, not
            // ShopPacks - see BattlePackShopEntries' doc comment (Blue Angel
            // confirmed, the other 3 an unconfirmed labeled-experimental
            // hypothesis).
            foreach (var entry in BattlePackShopEntries)
                ShopPackItems.Add(new CheckableItem(entry.Label, 0, entry.Kind, (bp & entry.Flag) != 0));

            var tut = MiscSaveLayout.GetTutorials(Bytes!, Version);
            for (int i = 1; i <= 20; i++)
            {
                var flag = (CompleteTutorials)(1 << TutorialBitShift(i));
                TutorialItems.Add(new CheckableItem($"Tutorial {i}", i - 1, "Tutorial", (tut & flag) != 0));
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private static int TutorialBitShift(int tutorialNumber) =>
        tutorialNumber <= 15 ? tutorialNumber : tutorialNumber + 1; // bit 16 is reserved ("Skipped")

    private void UnlockAllShopPacks_Click(object sender, RoutedEventArgs e) => BulkShopPacks(unlock: true);
    private void ClearAllShopPacks_Click(object sender, RoutedEventArgs e) => BulkShopPacks(unlock: false);

    /// <summary>Covers both fields the Shop Packs list actually touches:
    /// ShopPacks for the normal per-duelist bits (plus Epic Dawn, bit 31),
    /// and BattlePacksFlag's 4 BattlePackShopEntries bits (Blue Angel
    /// confirmed, Soulburner/Varis/Ai an unconfirmed hypothesis - see that
    /// array's doc comment) - all 4 are listed here alongside the other shop
    /// duelists since that's where players expect them, even though their
    /// real flags live elsewhere. WarOfTheGiants/WarOfTheGiantsRound2 aren't
    /// touched here - those stay under the Battle Packs section's own bulk
    /// buttons.</summary>
    private void BulkShopPacks(bool unlock)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetShopPacks(Bytes!, Version, unlock ? UnlockedShopPacks.All : UnlockedShopPacks.None);
        var bp = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
        foreach (var entry in BattlePackShopEntries)
            bp = unlock ? bp | entry.Flag : bp & ~entry.Flag;
        MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, bp);
        RaiseSaveDataChanged();
        RefreshUnlocksTab();
    }

    private void UnlockAllTutorials_Click(object sender, RoutedEventArgs e) => BulkTutorials(unlock: true);
    private void ClearAllTutorials_Click(object sender, RoutedEventArgs e) => BulkTutorials(unlock: false);

    private void BulkTutorials(bool unlock)
    {
        if (!HasSave) return;

        CompleteTutorials value = CompleteTutorials.None;
        if (unlock)
        {
            for (int i = 1; i <= 20; i++)
                value |= (CompleteTutorials)(1 << TutorialBitShift(i));
        }

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetTutorials(Bytes!, Version, value);
        RaiseSaveDataChanged();
        RefreshUnlocksTab();
    }

    private void UnlockPadlocked_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.UnlockPadlockedContent(Bytes!, Version);
        RaiseSaveDataChanged();
        RefreshUnlocksTab();
    }

    private void ContentFlag_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        var current = MiscSaveLayout.GetUnlockedContent(Bytes!, Version);
        UnlockedContent flag = ReferenceEquals(sender, ContentChallengesCheck) ? UnlockedContent.DuelistChallenges
            : ReferenceEquals(sender, ContentBattlePackCheck) ? UnlockedContent.BattlePack
            : UnlockedContent.CardShop;
        bool on = (sender as CheckBox)?.IsChecked == true;

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetUnlockedContent(Bytes!, Version, on ? current | flag : current & ~flag);
        RaiseSaveDataChanged();
    }

    private void UnlockAllBattlePacks_Click(object sender, RoutedEventArgs e) => BulkBattlePacks(unlock: true);
    private void ClearAllBattlePacks_Click(object sender, RoutedEventArgs e) => BulkBattlePacks(unlock: false);

    /// <summary>Sets both fields that back the 3 Battle Pack checkboxes:
    /// BattlePacksFlag's exact 0x3F/0x00 pattern (confirmed against a real
    /// fully-unlocked save - see UnlockedBattlePacks' doc comment; bits 3-5's
    /// individual meaning isn't confirmed but they're demonstrably part of a
    /// real "everything unlocked" save) PLUS ShopPacks' EpicDawn bit (its real
    /// location, confirmed via a real before/after diff - see
    /// UnlockedShopPacks' doc comment). The 3 checkboxes below only ever
    /// touch their own single bit each, so this button is the only way to
    /// reach that exact real-save state in one step.</summary>
    private void BulkBattlePacks(bool unlock)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, unlock ? UnlockedBattlePacks.All : 0);
        var shop = MiscSaveLayout.GetShopPacks(Bytes!, Version);
        MiscSaveLayout.SetShopPacks(Bytes!, Version, unlock ? shop | UnlockedShopPacks.EpicDawn : shop & ~UnlockedShopPacks.EpicDawn);
        RaiseSaveDataChanged();
        RefreshUnlocksTab();
    }

    /// <summary>Epic Dawn is a special case: its checkbox is visually grouped
    /// with the other 2 Battle Packs, but its real flag lives in ShopPacks
    /// (bit 31), not BattlePacksFlag - see UnlockedShopPacks' doc comment for
    /// the before/after save diff that proved this.</summary>
    private void BattlePackFlag_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;

        if (ReferenceEquals(sender, BpEpicDawnCheck))
        {
            bool EDon = BpEpicDawnCheck.IsChecked == true;
            var currentShop = MiscSaveLayout.GetShopPacks(Bytes!, Version);
            AppContext.Undo.Snapshot();
            MiscSaveLayout.SetShopPacks(Bytes!, Version, EDon ? currentShop | UnlockedShopPacks.EpicDawn : currentShop & ~UnlockedShopPacks.EpicDawn);
            RaiseSaveDataChanged();
            return;
        }

        var current = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
        UnlockedBattlePacks flag = ReferenceEquals(sender, BpWotgCheck) ? UnlockedBattlePacks.WarOfTheGiants
            : UnlockedBattlePacks.WarOfTheGiantsRound2;
        bool on = (sender as CheckBox)?.IsChecked == true;

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, on ? current | flag : current & ~flag);
        RaiseSaveDataChanged();
    }

    /// <summary>Shared Checked/Unchecked handler for Shop Packs, Tutorials
    /// and Avatars - CheckableItem.Kind says which underlying flag store to
    /// write to (ShopPacks/Tutorials/UnlockedAvatars directly, or
    /// BattlePacksFlag by way of BattlePackShopEntries for the 4 shop
    /// duelists whose real flag lives there instead).</summary>
    private void CheckableItem_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        if ((sender as FrameworkElement)?.DataContext is not CheckableItem item) return;
        bool on = (sender as CheckBox)?.IsChecked == true;

        AppContext.Undo.Snapshot();
        switch (item.Kind)
        {
            case "ShopPack":
            {
                // Each row is fully independent - one checkbox unlocks/locks
                // exactly one shop pack's flag, nothing else.
                var current = MiscSaveLayout.GetShopPacks(Bytes!, Version);
                var flag = (UnlockedShopPacks)ShopPackLabels[item.Index].Flag;
                MiscSaveLayout.SetShopPacks(Bytes!, Version, on ? current | flag : current & ~flag);
                break;
            }
            case "Tutorial":
            {
                var current = MiscSaveLayout.GetTutorials(Bytes!, Version);
                var flag = (CompleteTutorials)(1 << TutorialBitShift(item.Index + 1));
                MiscSaveLayout.SetTutorials(Bytes!, Version, on ? current | flag : current & ~flag);
                break;
            }
            case "Avatar":
                MiscSaveLayout.SetAvatarUnlocked(Bytes!, Version, item.Index, on);
                break;
            default:
            {
                // BlueAngel/Soulburner/Varis/Ai: sold as normal duelist shop
                // packs in-game, but their real flags live in BattlePacksFlag,
                // not ShopPacks - see BattlePackShopEntries' doc comment.
                foreach (var entry in BattlePackShopEntries)
                {
                    if (entry.Kind != item.Kind) continue;
                    var current = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
                    MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, on ? current | entry.Flag : current & ~entry.Flag);
                    break;
                }
                break;
            }
        }
        RaiseSaveDataChanged();
    }

    private void UnlockChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Available);
    private void CompleteChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Complete);
    private void ResetChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Locked);

    private void BulkChallenges(DeulistChallengeState state)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetAllChallenges(Bytes!, Version, state);
        RaiseSaveDataChanged();
    }

    private void UnlockRecipes_Click(object sender, RoutedEventArgs e) => BulkRecipes(true);
    private void LockRecipes_Click(object sender, RoutedEventArgs e) => BulkRecipes(false);

    private void BulkRecipes(bool unlocked)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetAllRecipesUnlocked(Bytes!, Version, unlocked);
        RaiseSaveDataChanged();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Avatars
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshAvatarsTab()
    {
        _suppressEvents = true;
        try
        {
            AvatarItems.Clear();
            if (!HasSave) return;

            for (int i = 0; i < LotdSaveFormat.NumAvatarSlots; i++)
            {
                string name = OwnerDatabase.GetName((byte)i);
                bool unlocked = MiscSaveLayout.GetAvatarUnlocked(Bytes!, Version, i);
                AvatarItems.Add(new CheckableItem(name, i, "Avatar", unlocked));
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private void UnlockAllAvatars_Click(object sender, RoutedEventArgs e) => BulkAvatars(true);
    private void LockAllAvatars_Click(object sender, RoutedEventArgs e) => BulkAvatars(false);

    private void BulkAvatars(bool unlocked)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetAllAvatarsUnlocked(Bytes!, Version, unlocked);
        RaiseSaveDataChanged();
        RefreshAvatarsTab();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Stats (see StatsLayout.cs - 42 known campaign/duel counters)
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshStatsTab()
    {
        StatItems.Clear();
        if (!HasSave) return;

        foreach (StatType stat in Enum.GetValues<StatType>())
        {
            long value = StatsLayout.Get(Bytes!, Version, stat);
            StatItems.Add(new StatRowViewModel(StatsLayout.GetLabel(stat), (int)stat, value));
        }
    }

    /// <summary>Commits one Stats row's TextBox back to the save on focus
    /// loss - same auto-commit-without-a-button convention as the
    /// checklists, just for a free-typed number instead of a checkbox. Skips
    /// the write entirely (no undo snapshot, no re-render) if the text
    /// doesn't parse or didn't actually change, so tabbing through every box
    /// without editing anything doesn't spam the undo stack.</summary>
    private void StatValue_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        if (sender is not TextBox box || box.DataContext is not StatRowViewModel row) return;
        if (!long.TryParse(box.Text, out long value)) { box.Text = row.Value; return; }
        value = Math.Max(value, 0L);

        long current = StatsLayout.Get(Bytes!, Version, row.Index);
        if (value == current) { box.Text = value.ToString(); return; }

        AppContext.Undo.Snapshot();
        StatsLayout.Set(Bytes!, Version, row.Index, value);
        RaiseSaveDataChanged();
        box.Text = value.ToString();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Card Collection (bulk only - see CardCollectionLayout.cs for why)
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshCardsTab(int? reselectCardLotdId = null)
    {
        BulkCountBox.IsEnabled = HasSave;
        CardSearchBox.IsEnabled = HasSave;
        if (!HasSave)
        {
            CardsSummaryText.Text = "— / — card slots have at least 1 copy owned.";
            RefreshCardSearchResults();
            return;
        }

        // Real cards are scattered throughout the full CardList chunk, not
        // clustered in a 0..GetNumCards-1 prefix (confirmed via a real save
        // with every real card owned - owned slots ran from index ~4007 to
        // ~14964). So the owned tally has to scan the full GetNumCardSlots
        // range; GetNumCards is used only as the display denominator (the
        // known real-card total), not as a scan bound.
        int total = CardCollectionLayout.GetNumCards(Version);
        int slots = CardCollectionLayout.GetNumCardSlots(Version);
        int owned = 0;
        for (int i = 0; i < slots; i++)
            if (CardCollectionLayout.GetCount(Bytes!, Version, i) > 0) owned++;

        CardsSummaryText.Text = $"{owned:N0} / {total:N0} card slots have at least 1 copy owned.";
        RefreshCardSearchResults(reselectCardLotdId);
    }

    /// <summary>Per-card unlock: searches AppContext.CardDb by name (same
    /// database CardSearchView browses) and shows a compact results list
    /// with each match's live owned count. LotdId is the byte index
    /// CardCollectionLayout.GetCount/SetCount actually take - confirmed
    /// 2026-07-23 to be the real save-slot index (see
    /// CardSearchResultViewModel's doc comment) - so only entries with a
    /// LotdId are searchable here; nameless/unmapped cards can't be targeted
    /// this way. Selecting a result (ListBox SelectionChanged) reveals
    /// CardSearchDetailPanel so its count can be edited and applied via
    /// SetSpecificCardCount_Click.</summary>
    private void RefreshCardSearchResults(int? reselectLotdId = null)
    {
        _suppressEvents = true;
        try
        {
            CardSearchResults.Clear();
            CardSearchDetailPanel.Visibility = Visibility.Collapsed;
            _selectedCardLotdId = null;

            string query = CardSearchBox.Text?.Trim() ?? string.Empty;
            if (!HasSave || query.Length < 2)
            {
                CardSearchStatusText.Text = "Type at least 2 characters to search.";
                return;
            }

            var matches = AppContext.CardDb.Search(query)
                .Where(c => c.LotdId.HasValue)
                .Take(CardSearchResultCap)
                .ToList();

            foreach (var card in matches)
            {
                int owned = CardCollectionLayout.GetCount(Bytes!, Version, card.LotdId!.Value);
                CardSearchResults.Add(new CardSearchResultViewModel(card, owned));
            }

            CardSearchStatusText.Text = matches.Count == 0 ? "No matches."
                : matches.Count == CardSearchResultCap ? $"{CardSearchResultCap}+ matches - refine your search"
                : $"{matches.Count} match{(matches.Count == 1 ? "" : "es")}";
        }
        finally
        {
            _suppressEvents = false;
        }

        if (reselectLotdId is int id)
        {
            var match = CardSearchResults.FirstOrDefault(c => c.LotdId == id);
            if (match != null) CardSearchResultsList.SelectedItem = match;
        }
    }

    private void CardSearchBox_TextChanged(object sender, TextChangedEventArgs e) => RefreshCardSearchResults();

    private void CardSearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressEvents) return;

        if (CardSearchResultsList.SelectedItem is not CardSearchResultViewModel vm)
        {
            CardSearchDetailPanel.Visibility = Visibility.Collapsed;
            _selectedCardLotdId = null;
            return;
        }

        _selectedCardLotdId = vm.LotdId;
        CardSearchSelectedName.Text = vm.Name;
        CardSearchCountBox.Text = vm.OwnedCount.ToString();
        CardSearchDetailPanel.Visibility = Visibility.Visible;
    }

    private void SetSpecificCardCount_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave || _selectedCardLotdId is not int lotdId) return;
        if (!byte.TryParse(CardSearchCountBox.Text, out byte count)) return;

        // Same 0-3 clamp as the bulk count box - the game only ever allows
        // up to 3 copies of a card.
        count = Math.Min(count, (byte)3);
        CardSearchCountBox.Text = count.ToString();

        AppContext.Undo.Snapshot();
        CardCollectionLayout.SetCount(Bytes!, Version, lotdId, count);
        // Setting a specific count > 0 is "unlocking" the card - clear its
        // "NEW" ribbon the same way UnlockAllCards does. Setting to 0 is
        // un-owning it, which shouldn't force Seen either way, so it's left
        // alone in that case.
        if (count > 0) CardCollectionLayout.SetSeen(Bytes!, Version, lotdId, true);
        RaiseSaveDataChanged();
        RefreshCardsTab(reselectCardLotdId: lotdId);
    }

    /// <summary>Unlocks only the real ~10027 cards Cards.json knows about
    /// (AppContext.CardDb.AllLotdIds()), not the full ~20000-slot chunk -
    /// CardCollectionLayout.UnlockAllCards touches every padding slot too,
    /// which used to make the Cards tab's counter read "20000/10027" after
    /// unlocking (bug reported/fixed 2026-07-23).</summary>
    private void UnlockAllCards_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        CardCollectionLayout.SetCardsCount(Bytes!, Version, AppContext.CardDb.AllLotdIds(), 3, seen: true);
        RaiseSaveDataChanged();
        RefreshCardsTab();
    }

    private void MarkAllSeen_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        CardCollectionLayout.MarkAllSeen(Bytes!, Version);
        RaiseSaveDataChanged();
        RefreshCardsTab();
    }

    /// <summary>Rewrites the count on every card the save ALREADY owns -
    /// cards currently at 0 stay at 0 (this button doesn't unlock anything;
    /// UNLOCK ALL CARDS above is the one that does). Uses
    /// CardCollectionLayout.SetOwnedCardsCount rather than
    /// SetAllOwnedCardsCount, which would touch every slot in the 20000-slot
    /// chunk regardless of current ownership and effectively "own" every
    /// padding/unowned slot too. RefreshCardsTab already recomputes the
    /// summary by scanning actual owned counts, so the "X/Y" figure updates
    /// correctly on its own - no separate counter fix needed here.</summary>
    private void ApplyBulkCount_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        if (!byte.TryParse(BulkCountBox.Text, out byte count)) return;

        // The game only ever allows up to 3 copies of a card - clamp here
        // rather than trusting free-typed input (CardCollectionLayout itself
        // would silently mask to 3 bits, i.e. up to 7, which isn't the same
        // guarantee).
        count = Math.Min(count, (byte)3);
        BulkCountBox.Text = count.ToString();

        AppContext.Undo.Snapshot();
        CardCollectionLayout.SetOwnedCardsCount(Bytes!, Version, count);
        RaiseSaveDataChanged();
        RefreshCardsTab();
    }

    private void ResetCollection_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;

        var owner = Window.GetWindow(this);
        var confirm = AppMessageBox.Show(owner,
            "Reset the entire card collection to 0 copies owned and unseen?",
            "Reset Card Collection", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        AppContext.Undo.Snapshot();
        CardCollectionLayout.ResetAllCards(Bytes!, Version);
        RaiseSaveDataChanged();
        RefreshCardsTab();
    }
}
