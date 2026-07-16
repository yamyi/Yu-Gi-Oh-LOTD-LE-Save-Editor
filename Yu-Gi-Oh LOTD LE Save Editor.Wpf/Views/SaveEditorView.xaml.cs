using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using YuGiOhSaveEditor.Wpf.Controls;
using YuGiOhSaveEditor.Wpf.Services;

namespace YuGiOhSaveEditor.Wpf.Views;

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

    private static readonly (UnlockedShopPacks Flag, string Label)[] ShopPackLabels =
    {
        (UnlockedShopPacks.GrandpaMuto, "Grandpa Muto"),
        (UnlockedShopPacks.MaiValentine, "Mai Valentine"),
        (UnlockedShopPacks.Bakura, "Bakura"),
        (UnlockedShopPacks.JoeyWheeler, "Joey Wheeler"),
        (UnlockedShopPacks.SetoKaiba, "Seto Kaiba"),
        (UnlockedShopPacks.Yugi, "Yugi"),
        (UnlockedShopPacks.AlexisRhodes, "Alexis Rhodes"),
        (UnlockedShopPacks.BastionMisawa, "Bastion Misawa"),
        (UnlockedShopPacks.ChazzPrinceton, "Chazz Princeton"),
        (UnlockedShopPacks.SyrusTruesdale, "Syrus Truesdale"),
        (UnlockedShopPacks.JesseAnderson, "Jesse Anderson"),
        (UnlockedShopPacks.JadenYuki, "Jaden Yuki"),
        (UnlockedShopPacks.TetsuTrudge, "Tetsu Trudge"),
        (UnlockedShopPacks.LeoLuna, "Leo/Luna"),
        (UnlockedShopPacks.AkizaIzinski, "Akiza Izinski"),
        (UnlockedShopPacks.JackAtlas, "Jack Atlas"),
        (UnlockedShopPacks.Crow, "Crow"),
        (UnlockedShopPacks.YuseiFudo, "Yusei Fudo"),
        (UnlockedShopPacks.CathyKatherine, "Cathy Katherine"),
        (UnlockedShopPacks.Quinton, "Quinton"),
        (UnlockedShopPacks.KiteTenjo, "Kite Tenjo"),
        (UnlockedShopPacks.Shark, "Shark"),
        (UnlockedShopPacks.YumaTsukumo, "Yuma Tsukumo"),
        (UnlockedShopPacks.Pendulum, "Pendulum (unlocks all ARC-V)"),
        (UnlockedShopPacks.GongStrong, "Gong Strong (unlocks all ARC-V)"),
        (UnlockedShopPacks.ZuzuBoyle, "Zuzu Boyle (unlocks all ARC-V)"),
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
                BpWotgRound2Check.IsChecked = false;
                return;
            }

            var content = MiscSaveLayout.GetUnlockedContent(Bytes!, Version);
            ContentChallengesCheck.IsChecked = (content & UnlockedContent.DuelistChallenges) != 0;
            ContentBattlePackCheck.IsChecked = (content & UnlockedContent.BattlePack) != 0;
            ContentCardShopCheck.IsChecked = (content & UnlockedContent.CardShop) != 0;

            var bp = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
            BpEpicDawnCheck.IsChecked = (bp & UnlockedBattlePacks.WarOfTheGiants) != 0;
            BpWotgRound2Check.IsChecked = (bp & UnlockedBattlePacks.WarOfTheGiantsRound2) != 0;

            var shop = MiscSaveLayout.GetShopPacks(Bytes!, Version);
            for (int i = 0; i < ShopPackLabels.Length; i++)
                ShopPackItems.Add(new CheckableItem(ShopPackLabels[i].Label, i, "ShopPack", (shop & ShopPackLabels[i].Flag) != 0));

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

    private void UnlockAllShopPacks_Click(object sender, RoutedEventArgs e) => BulkShopPacks(UnlockedShopPacks.All);
    private void ClearAllShopPacks_Click(object sender, RoutedEventArgs e) => BulkShopPacks(UnlockedShopPacks.None);

    private void BulkShopPacks(UnlockedShopPacks value)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetShopPacks(Bytes!, Version, value);
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

    private void BattlePackFlag_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        var current = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
        UnlockedBattlePacks flag = ReferenceEquals(sender, BpEpicDawnCheck)
            ? UnlockedBattlePacks.WarOfTheGiants
            : UnlockedBattlePacks.WarOfTheGiantsRound2;
        bool on = (sender as CheckBox)?.IsChecked == true;

        AppContext.Undo.Snapshot();
        MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, on ? current | flag : current & ~flag);
        RaiseSaveDataChanged();
    }

    /// <summary>Shared Checked/Unchecked handler for Shop Packs, Tutorials
    /// and Avatars - CheckableItem.Kind says which of the three underlying
    /// flag stores to write to.</summary>
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
                var current = MiscSaveLayout.GetShopPacks(Bytes!, Version);
                var flag = ShopPackLabels[item.Index].Flag;
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
    // Card Collection (bulk only - see CardCollectionLayout.cs for why)
    // ══════════════════════════════════════════════════════════════════════

    private void RefreshCardsTab()
    {
        BulkCountBox.IsEnabled = HasSave;
        if (!HasSave)
        {
            CardsSummaryText.Text = "— / — card slots have at least 1 copy owned.";
            return;
        }

        int total = CardCollectionLayout.GetNumCards(Version);
        int owned = 0;
        for (int i = 0; i < total; i++)
            if (CardCollectionLayout.GetCount(Bytes!, Version, i) > 0) owned++;

        CardsSummaryText.Text = $"{owned:N0} / {total:N0} card slots have at least 1 copy owned.";
    }

    private void UnlockAllCards_Click(object sender, RoutedEventArgs e)
    {
        if (!HasSave) return;
        AppContext.Undo.Snapshot();
        CardCollectionLayout.UnlockAllCards(Bytes!, Version);
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
        CardCollectionLayout.SetAllOwnedCardsCount(Bytes!, Version, count);
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
