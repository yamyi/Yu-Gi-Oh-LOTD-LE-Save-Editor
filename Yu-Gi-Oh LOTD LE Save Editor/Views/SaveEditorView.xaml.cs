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
    private int _selectedChallengeSeriesIndex;
    private int _selectedAvatarSeriesIndex;
    private string _currentTabKey = "DuelPoints";

    private byte[]? Bytes => AppContext.State?.SaveBytes;
    private LotdSaveVersion Version => AppContext.State?.Version ?? LotdSaveVersion.LinkEvolution;
    private bool HasSave => Bytes is { Length: > 0 };

    private Dictionary<string, (Button Button, Grid Section)> _tabs = new();
    private Dictionary<LotdDuelSeries, Button> _seriesButtons = new();
    private Dictionary<LotdDuelSeries, Button> _challengeSeriesButtons = new();
    private Dictionary<LotdDuelSeries, Button> _avatarSeriesButtons = new();

    public ObservableCollection<DuelRowViewModel> DuelRows { get; } = new();
    public ObservableCollection<CheckableItem> ShopPackItems { get; } = new();
    public ObservableCollection<CheckableItem> TutorialItems { get; } = new();
    public ObservableCollection<CheckableItem> AvatarItems { get; } = new();
    public ObservableCollection<StatRowViewModel> StatItems { get; } = new();
    public ObservableCollection<CardSearchResultViewModel> CardSearchResults { get; } = new();
    public ObservableCollection<ChallengeRowViewModel> ChallengeItems { get; } = new();

    private const int CardSearchResultCap = 40;
    private int? _selectedCardLotdId;

    private static readonly string[] DuelStateOptions =
        { "Locked", "Available", "AvailableAttempted", "Complete", "AvailableAlt" };

    /// <summary>Bound via ElementName=Root from DuelRowTemplate's two
    /// ComboBoxes - a plain static option list, same convention as every
    /// other cross-element binding in this app.</summary>
    public string[] DuelStateNames => DuelStateOptions;

    private static readonly string[] ChallengeStateOptions =
        { "Locked", "Available", "Failed", "Complete" };

    /// <summary>Bound via ElementName=Root from ChallengeRowTemplate's
    /// ComboBox - DeulistChallengeState's 4 named values in on-disk order.</summary>
    public string[] ChallengeStateNames => ChallengeStateOptions;

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
            ["Challenges"] = (TabChallengesButton, ChallengesSection),
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

        _challengeSeriesButtons = new Dictionary<LotdDuelSeries, Button>
        {
            [LotdDuelSeries.YuGiOh] = ChallengeSeriesYuGiOhButton,
            [LotdDuelSeries.YuGiOhGX] = ChallengeSeriesGXButton,
            [LotdDuelSeries.YuGiOh5D] = ChallengeSeries5DButton,
            [LotdDuelSeries.YuGiOhZEXAL] = ChallengeSeriesZEXALButton,
            [LotdDuelSeries.YuGiOhARCV] = ChallengeSeriesARCVButton,
            [LotdDuelSeries.YuGiOhVRAINS] = ChallengeSeriesVRAINSButton,
        };

        _avatarSeriesButtons = new Dictionary<LotdDuelSeries, Button>
        {
            [LotdDuelSeries.YuGiOh] = AvatarSeriesYuGiOhButton,
            [LotdDuelSeries.YuGiOhGX] = AvatarSeriesGXButton,
            [LotdDuelSeries.YuGiOh5D] = AvatarSeries5DButton,
            [LotdDuelSeries.YuGiOhZEXAL] = AvatarSeriesZEXALButton,
            [LotdDuelSeries.YuGiOhARCV] = AvatarSeriesARCVButton,
            [LotdDuelSeries.YuGiOhVRAINS] = AvatarSeriesVRAINSButton,
        };

        DuelRowsList.ItemsSource = DuelRows;
        ShopPacksList.ItemsSource = ShopPackItems;
        TutorialsList.ItemsSource = TutorialItems;
        AvatarsList.ItemsSource = AvatarItems;
        StatsList.ItemsSource = StatItems;
        CardSearchResultsList.ItemsSource = CardSearchResults;
        ChallengesList.ItemsSource = ChallengeItems;

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
        RefreshChallengesTab();
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

    // Real duel ranges per series now live on CampaignSaveLayout.KnownRealDuelRange
    // (also consumed by StatsLayout.RecalculateCampaignStats) so the UI filter and
    // the stat recalculation can never disagree about which slots are real duels.

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
            if (currentSeries.HasValue && CampaignSaveLayout.KnownRealDuelRange.TryGetValue(currentSeries.Value, out var range))
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

        // A duel (forward or reverse) reading anything but Locked implies
        // every duel before it must already be at least that progressed -
        // see CampaignSaveLayout.EnsurePrerequisitesComplete's doc comment.
        if (state != LotdCampaignDuelState.Locked)
            CampaignSaveLayout.EnsurePrerequisitesComplete(Bytes!, Version, _selectedSeriesIndex, row.DuelIndex);

        // Keep the lifetime Stats chunk from drifting away from whatever the
        // duel grid now says - see StatsLayout.RecalculateCampaignStats' doc
        // comment for why this used to be the cause of saves getting stuck on
        // the post-duel results screen.
        StatsLayout.RecalculateCampaignStats(Bytes!, Version);
        // A story stage flipping to/from Complete auto-(un)locks its
        // milestone duelist's shop/battle pack - see
        // MiscSaveLayout.SyncShopPacksFromCampaignState's doc comment.
        MiscSaveLayout.SyncShopPacksFromCampaignState(Bytes!, Version);
        // A character's Campaign duel (forward + reverse) going all-Complete
        // auto-unlocks their Challenge, which can in turn unlock the
        // Duelist Challenges gate - see MiscSaveLayout.SyncChallengesFromCampaignState
        // and SyncChallengeGateFromChallenges' doc comments.
        MiscSaveLayout.SyncChallengesFromCampaignState(Bytes!, Version);
        MiscSaveLayout.SyncChallengeGateFromChallenges(Bytes!, Version);
        // Same rule, applied to avatars instead of challenges - see
        // MiscSaveLayout.SyncAvatarsFromCampaignState's doc comment.
        MiscSaveLayout.SyncAvatarsFromCampaignState(Bytes!, Version);
        RefreshStatsTab();
        RefreshUnlocksTab();
        RefreshChallengesTab();
        RefreshAvatarsTab();
        RaiseSaveDataChanged();
        // Refresh the duel grid too - EnsurePrerequisitesComplete above may
        // have just completed earlier duel cards that are still on screen.
        RebuildDuelGrid();
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
        StatsLayout.RecalculateCampaignStats(Bytes!, Version);
        MiscSaveLayout.SyncShopPacksFromCampaignState(Bytes!, Version);
        MiscSaveLayout.SyncChallengesFromCampaignState(Bytes!, Version);
        MiscSaveLayout.SyncChallengeGateFromChallenges(Bytes!, Version);
        MiscSaveLayout.SyncAvatarsFromCampaignState(Bytes!, Version);
        RefreshStatsTab();
        RefreshUnlocksTab();
        RefreshChallengesTab();
        RefreshAvatarsTab();
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
        StatsLayout.RecalculateCampaignStats(Bytes!, Version);
        MiscSaveLayout.SyncShopPacksFromCampaignState(Bytes!, Version);
        MiscSaveLayout.SyncChallengesFromCampaignState(Bytes!, Version);
        MiscSaveLayout.SyncChallengeGateFromChallenges(Bytes!, Version);
        MiscSaveLayout.SyncAvatarsFromCampaignState(Bytes!, Version);
        RefreshStatsTab();
        RefreshUnlocksTab();
        RefreshChallengesTab();
        RefreshAvatarsTab();
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
    /// hardcoded case per duelist.</summary>
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
            // BattlePacksFlag - see UnlockedShopPacks' doc comment.
            BpEpicDawnCheck.IsChecked = (shop & (uint)UnlockedShopPacks.EpicDawn) != 0;
            for (int i = 0; i < ShopPackLabels.Length; i++)
            {
                var entry = ShopPackLabels[i];
                bool isChecked = (shop & entry.Flag) != 0;
                ShopPackItems.Add(new CheckableItem(entry.Label, i, "ShopPack", isChecked));
            }
            // Blue Angel/Soulburner/Varis/Ai are sold as normal duelist shop
            // packs in-game, but their real flags live in BattlePacksFlag,
            // not ShopPacks - see BattlePackShopEntries' doc comment.
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
    /// and BattlePacksFlag's 4 BattlePackShopEntries bits - all 4 are listed
    /// here alongside the other shop duelists since that's where players
    /// expect them, even though their real flags live elsewhere.
    /// WarOfTheGiants/WarOfTheGiantsRound2 aren't touched here - those stay
    /// under the Battle Packs section's own bulk buttons.</summary>
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
    /// BattlePacksFlag's exact 0x3F/0x00 pattern (see UnlockedBattlePacks'
    /// doc comment) plus ShopPacks' EpicDawn bit (see UnlockedShopPacks' doc
    /// comment). The 3 checkboxes below only ever touch their own single bit
    /// each, so this button is the only way to reach that exact real-save
    /// state in one step.</summary>
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
    /// (bit 31), not BattlePacksFlag - see UnlockedShopPacks' doc
    /// comment.</summary>
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
    /// duelists whose real flag lives there instead).
    ///
    /// Failsafe: checking a box ON is refused (reverted, no write, no undo
    /// snapshot) unless CanManuallyUnlockCheckableItem says its real
    /// Campaign prerequisite is already satisfied. Unchecking (locking) is
    /// never refused.</summary>
    private void CheckableItem_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        if ((sender as FrameworkElement)?.DataContext is not CheckableItem item) return;
        if (sender is not CheckBox checkBox) return;
        bool on = checkBox.IsChecked == true;

        if (on && !CanManuallyUnlockCheckableItem(item, out string blockedReason))
        {
            _suppressEvents = true;
            checkBox.IsChecked = false;
            _suppressEvents = false;
            AppMessageBox.Show(Window.GetWindow(this), blockedReason, "Not Unlockable Yet",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

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

    /// <summary>Failsafe gate for CheckableItem_Changed's unlock direction -
    /// see that method's doc comment. Only ShopPack/Avatar/the 4
    /// BattlePackShopEntries kinds are gated (everything with a real
    /// Campaign prerequisite); Tutorial and anything else fall through to
    /// "always allowed" since this app has no campaign-driven rule for
    /// them.</summary>
    private bool CanManuallyUnlockCheckableItem(CheckableItem item, out string blockedReason)
    {
        blockedReason = string.Empty;

        switch (item.Kind)
        {
            case "ShopPack":
            {
                var entry = ShopPackLabels[item.Index];
                if (MiscSaveLayout.CanManuallyUnlockShopPack(Bytes!, Version, MiscSaveLayout.PackFlagKind.Shop, entry.Flag))
                    return true;
                blockedReason = $"{entry.Label}'s shop pack unlocks after completing their Campaign story stage - complete it first.";
                return false;
            }
            case "Avatar":
            {
                if (MiscSaveLayout.CanManuallyUnlock(Bytes!, Version, (byte)item.Index))
                    return true;
                blockedReason = $"{item.Label}'s avatar unlocks after completing all of their Campaign duels (forward and reverse) - complete them first.";
                return false;
            }
            default:
            {
                foreach (var entry in BattlePackShopEntries)
                {
                    if (entry.Kind != item.Kind) continue;
                    if (MiscSaveLayout.CanManuallyUnlockShopPack(Bytes!, Version, MiscSaveLayout.PackFlagKind.Battle, (uint)entry.Flag))
                        return true;
                    blockedReason = $"{entry.Label}'s shop pack unlocks after completing their Campaign story stage - complete it first.";
                    return false;
                }
                return true; // Tutorial, or any other kind this failsafe doesn't cover
            }
        }
    }

    private void UnlockChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Available);
    private void CompleteChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Complete);
    private void ResetChallenges_Click(object sender, RoutedEventArgs e) => BulkChallenges(DeulistChallengeState.Locked);

    private void BulkChallenges(DeulistChallengeState state)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetAllChallenges(Bytes!, Version, state);
        // Same reasoning as the Campaign bulk buttons: keep Games_Challenge/
        // Wins_Challenge (and Wins_Nonmatch, derived from it) from drifting
        // away from whatever the challenge slots actually say now.
        StatsLayout.RecalculateChallengeStats(Bytes!, Version);
        // See MiscSaveLayout.SyncChallengeGateFromChallenges' doc comment -
        // UNLOCK/COMPLETE can newly satisfy "at least 1 unlocked"; RESET
        // deliberately does NOT re-lock the gate (one-directional).
        MiscSaveLayout.SyncChallengeGateFromChallenges(Bytes!, Version);
        RefreshStatsTab();
        RefreshUnlocksTab();
        RefreshChallengesTab();
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
        if (HasSave)
        {
            var series = CampaignSaveLayout.GetSeries(Version);
            if (_selectedAvatarSeriesIndex >= series.Length) _selectedAvatarSeriesIndex = 0;
        }

        ApplyAvatarSeriesButtonVisuals();
        RebuildAvatarGrid();
    }

    /// <summary>Same Style-swap/visibility pattern as Campaign's
    /// ApplySeriesButtonVisuals and Challenges' ApplyChallengeSeriesButtonVisuals,
    /// against this tab's own button dictionary/selection.</summary>
    private void ApplyAvatarSeriesButtonVisuals()
    {
        var active = (Style)FindResource("SeriesLogoActiveStyle");
        var inactive = (Style)FindResource("SeriesLogoInactiveStyle");
        var series = HasSave ? CampaignSaveLayout.GetSeries(Version) : Array.Empty<LotdDuelSeries>();
        LotdDuelSeries? currentSeries = _selectedAvatarSeriesIndex >= 0 && _selectedAvatarSeriesIndex < series.Length
            ? series[_selectedAvatarSeriesIndex]
            : null;

        foreach (var (seriesValue, button) in _avatarSeriesButtons)
        {
            button.Visibility = Array.IndexOf(series, seriesValue) >= 0 ? Visibility.Visible : Visibility.Collapsed;
            button.Style = seriesValue == currentSeries ? active : inactive;
        }
    }

    private void AvatarSeriesLogoButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not string tag || !Enum.TryParse<LotdDuelSeries>(tag, out var series)) return;

        var allSeries = CampaignSaveLayout.GetSeries(Version);
        int idx = Array.IndexOf(allSeries, series);
        if (idx < 0) return;

        _selectedAvatarSeriesIndex = idx;
        ApplyAvatarSeriesButtonVisuals();
        RebuildAvatarGrid();
    }

    /// <summary>Populates AvatarItems for just the currently-selected series
    /// (DuelistChallengeSlots.GetSeries applied to the avatar id, which
    /// shares the same CharacterId space - see RefreshAvatarsTab's previous
    /// flat-list version, which already indexed OwnerDatabase the same way),
    /// same filter-to-one-series-at-a-time layout as Campaign/Challenges.
    /// Avatar ids only go up to LotdSaveFormat.NumAvatarSlots-1 (152), so
    /// later ZEXAL/VRAINS characters simply never appear in any series'
    /// list here - same limitation SyncAvatarsFromCampaignState documents.</summary>
    private void RebuildAvatarGrid()
    {
        _suppressEvents = true;
        try
        {
            AvatarItems.Clear();
            if (!HasSave) return;

            var allSeries = CampaignSaveLayout.GetSeries(Version);
            LotdDuelSeries? currentSeries = _selectedAvatarSeriesIndex >= 0 && _selectedAvatarSeriesIndex < allSeries.Length
                ? allSeries[_selectedAvatarSeriesIndex]
                : null;
            if (currentSeries is null) return;

            for (int i = 0; i < LotdSaveFormat.NumAvatarSlots; i++)
            {
                if (DuelistChallengeSlots.GetSeries((byte)i) != currentSeries.Value) continue;

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

        // Editing Wins_Nonmatch directly auto-(un)locks the Battle Pack
        // milestones gated on it - see
        // StatsLayout.SyncBattlePackUnlocksFromNonmatchWins' doc comment.
        // (Editing it indirectly, via Campaign/Challenge state changes that
        // call RecalculateCampaignStats/RecalculateChallengeStats, already
        // triggers the same sync from inside RecalculateNonmatchWins.)
        if (row.Index == (int)StatType.Wins_Nonmatch)
        {
            StatsLayout.SyncBattlePackUnlocksFromNonmatchWins(Bytes!, Version);
            RefreshUnlocksTab();
        }

        RaiseSaveDataChanged();
        box.Text = value.ToString();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Challenges (individual duelists - see DuelistChallengeSlots.cs for the
    // CharacterId -> Challenges-array slot mapping this tab is built from)
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshChallengesTab()
    {
        if (HasSave)
        {
            var series = CampaignSaveLayout.GetSeries(Version);
            if (_selectedChallengeSeriesIndex >= series.Length) _selectedChallengeSeriesIndex = 0;
        }

        ApplyChallengeSeriesButtonVisuals();
        RebuildChallengeGrid();
    }

    /// <summary>Same Style-swap/visibility pattern as Campaign's
    /// ApplySeriesButtonVisuals, just against this tab's own button
    /// dictionary/selection so the two series pickers don't interfere with
    /// each other.</summary>
    private void ApplyChallengeSeriesButtonVisuals()
    {
        var active = (Style)FindResource("SeriesLogoActiveStyle");
        var inactive = (Style)FindResource("SeriesLogoInactiveStyle");
        var series = HasSave ? CampaignSaveLayout.GetSeries(Version) : Array.Empty<LotdDuelSeries>();
        LotdDuelSeries? currentSeries = _selectedChallengeSeriesIndex >= 0 && _selectedChallengeSeriesIndex < series.Length
            ? series[_selectedChallengeSeriesIndex]
            : null;

        foreach (var (seriesValue, button) in _challengeSeriesButtons)
        {
            button.Visibility = Array.IndexOf(series, seriesValue) >= 0 ? Visibility.Visible : Visibility.Collapsed;
            button.Style = seriesValue == currentSeries ? active : inactive;
        }
    }

    private void ChallengeSeriesLogoButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.Tag is not string tag || !Enum.TryParse<LotdDuelSeries>(tag, out var series)) return;

        var allSeries = CampaignSaveLayout.GetSeries(Version);
        int idx = Array.IndexOf(allSeries, series);
        if (idx < 0) return;

        _selectedChallengeSeriesIndex = idx;
        ApplyChallengeSeriesButtonVisuals();
        RebuildChallengeGrid();
    }

    /// <summary>Populates ChallengeItems for just the currently-selected
    /// series (DuelistChallengeSlots.GetSeries), same
    /// filter-to-one-series-at-a-time layout as Campaign's RebuildDuelGrid.</summary>
    private void RebuildChallengeGrid()
    {
        _suppressEvents = true;
        try
        {
            ChallengeItems.Clear();
            if (!HasSave) return;

            var allSeries = CampaignSaveLayout.GetSeries(Version);
            LotdDuelSeries? currentSeries = _selectedChallengeSeriesIndex >= 0 && _selectedChallengeSeriesIndex < allSeries.Length
                ? allSeries[_selectedChallengeSeriesIndex]
                : null;
            if (currentSeries is null) return;

            // Slots at/beyond the real Challenges-array size are filtered out
            // here rather than left for MiscSaveLayout's bounds check to catch.
            int numSlots = LotdSaveFormat.GetNumDeckDataSlots(Version);
            var visible = DuelistChallengeSlots.Entries
                .Where(entry => entry.SlotIndex < numSlots && DuelistChallengeSlots.GetSeries(entry.CharacterId) == currentSeries.Value)
                .ToArray();

            // A handful of duelists have two distinct challenges sharing one
            // display name (see DuelistChallengeSlots' doc comment) - tag only
            // those with "(#CharacterId)" so the list stays uncluttered for
            // everyone else. Every real name collision happens to land in
            // the same series (The Gore, Varis - both VRAINS), so comparing
            // within just this series' visible set is enough; the other
            // collisions (Alexis Rhodes, Crow Hogan, Jack Atlas, Kite Tenjo,
            // Aster Phoenix) each span two different series and so never
            // collide within a single series' list to begin with.
            var nameCounts = visible
                .GroupBy(entry => OwnerDatabase.GetName(entry.CharacterId))
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var entry in visible)
            {
                string name = OwnerDatabase.GetName(entry.CharacterId);
                string label = nameCounts[name] > 1 ? $"{name} (#{entry.CharacterId})" : name;
                var state = MiscSaveLayout.GetChallengeState(Bytes!, Version, entry.SlotIndex);
                ChallengeItems.Add(new ChallengeRowViewModel(label, entry.CharacterId, entry.SlotIndex, DisplayChallengeState(state)));
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    /// <summary>Maps a raw challenge-state value to one of the 4 named
    /// states - same defensive clamp as Campaign's DisplayState, for any
    /// never-touched slot holding a raw value outside the enum.</summary>
    private static string DisplayChallengeState(DeulistChallengeState state)
    {
        int raw = (int)state;
        if (raw < 0) return DeulistChallengeState.Locked.ToString();
        if (raw > (int)DeulistChallengeState.Complete) return DeulistChallengeState.Complete.ToString();
        return state.ToString();
    }

    /// <summary>Commits one Challenges row's ComboBox back to the save on
    /// selection change - same auto-commit-without-a-button convention as
    /// every other checklist/dropdown in this view. Recalculates
    /// Games_Challenge/Wins_Challenge (and transitively Wins_Nonmatch, and
    /// transitively the Battle Pack milestones gated on that - see
    /// StatsLayout.SyncBattlePackUnlocksFromNonmatchWins) exactly like the
    /// existing bulk Unlock/Complete/Reset Challenges buttons already do, so
    /// unlocking a single named challenge here can never leave those derived
    /// values out of step with what this tab now shows.
    ///
    /// Failsafe: the specific Locked -> anything-else transition is refused
    /// (reverted, no write, no undo snapshot) unless
    /// MiscSaveLayout.CanManuallyUnlock says this duelist's Campaign duels
    /// are all Complete. Moving between two already-unlocked states (e.g.
    /// Available -> Complete) or back to Locked is never refused - only the
    /// initial unlock is gated.</summary>
    private void ChallengeState_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        if (sender is not ComboBox combo || combo.DataContext is not ChallengeRowViewModel row) return;
        if (combo.SelectedItem is not string text || !Enum.TryParse<DeulistChallengeState>(text, out var state)) return;

        var current = MiscSaveLayout.GetChallengeState(Bytes!, Version, row.SlotIndex);
        if (state == current) return;

        bool isUnlocking = current == DeulistChallengeState.Locked && state != DeulistChallengeState.Locked;
        if (isUnlocking && !MiscSaveLayout.CanManuallyUnlock(Bytes!, Version, row.CharacterId))
        {
            _suppressEvents = true;
            combo.SelectedItem = DisplayChallengeState(current);
            _suppressEvents = false;
            AppMessageBox.Show(Window.GetWindow(this),
                $"{row.Label}'s challenge unlocks after completing all of their Campaign duels (forward and reverse) - complete them first.",
                "Not Unlockable Yet", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetChallengeState(Bytes!, Version, row.SlotIndex, state);
        StatsLayout.RecalculateChallengeStats(Bytes!, Version);
        // Unlocking (or completing) this one challenge may be the save's
        // first non-Locked challenge - see
        // MiscSaveLayout.SyncChallengeGateFromChallenges' doc comment.
        MiscSaveLayout.SyncChallengeGateFromChallenges(Bytes!, Version);
        RefreshStatsTab();
        RefreshUnlocksTab();
        RaiseSaveDataChanged();
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
        // clustered in a 0..GetNumCards-1 prefix, so the owned tally has to
        // scan the full GetNumCardSlots range; GetNumCards is used only as
        // the display denominator (the known real-card total), not as a
        // scan bound.
        int total = CardCollectionLayout.GetNumCards(Version);
        int slots = CardCollectionLayout.GetNumCardSlots(Version);
        int owned = 0;
        int copies = 0;
        for (int i = 0; i < slots; i++)
        {
            int count = CardCollectionLayout.GetCount(Bytes!, Version, i);
            if (count > 0) owned++;
            copies += count;
        }

        // Second figure alongside the slot count: total individual copies
        // owned (a card counts up to 3 toward this), out of the maximum
        // possible if every one of the `total` real cards were owned at the
        // 3-copy cap - a "copies" tally distinct from the "at least 1 copy"
        // slot tally.
        int maxCopies = total * CardCollectionLayout.MaxCountPerCard;
        CardsSummaryText.Text =
            $"{owned:N0} / {total:N0} card slots have at least 1 copy owned   •   {copies:N0} / {maxCopies:N0} total copies owned";
        RefreshCardSearchResults(reselectCardLotdId);
    }

    /// <summary>Per-card unlock: searches AppContext.CardDb by name (same
    /// database CardSearchView browses) and shows a compact results list
    /// with each match's live owned count. LotdId is the byte index
    /// CardCollectionLayout.GetCount/SetCount actually take (see
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
    /// which would inflate the Cards tab's counter past the real card
    /// total.</summary>
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
