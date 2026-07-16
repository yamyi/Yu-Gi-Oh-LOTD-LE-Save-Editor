using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>
/// Full save-data editor: duel points, campaign duel unlocks, shop/battle pack
/// and avatar/tutorial unlocks, and bulk card-collection operations. Everything
/// here operates directly on AppContext.State.SaveBytes via the MiscSaveLayout /
/// CampaignSaveLayout / CardCollectionLayout helpers (Models/Save/), which are
/// ports of pixeltris/Lotd's own save-format code.
///
/// Every control on this page is declared and laid out in
/// SaveEditorPage.Designer.cs (same as every other page in this app) — nothing
/// here creates new Controls at runtime. Lists with many similar entries (shop
/// packs, tutorials, avatars, the 50-duel campaign grid) use a single
/// CheckedListBox/DataGridView control whose *rows/items* are populated in a
/// loop; that's ordinary data population, not runtime control creation.
///
/// Every mutation snapshots the undo stack first (BeginEdit) and raises
/// DataChanged afterwards so MainForm can mark the file dirty, exactly like
/// DeckSlotsPage/DeckEditorPage already do. A _suppressEvents flag guards every
/// place this page writes UI state back onto itself (refreshing a checkbox,
/// repopulating a list) so that sync-from-save doesn't get misread as a
/// user edit and write straight back into the save.
/// </summary>
public sealed partial class SaveEditorPage : UserControl
{
    public event EventHandler? DataChanged;

    private bool _suppressEvents;
    private int _selectedSeriesIndex;

    /// <summary>
    /// Maps a visible dgvDuels row index to the actual 0-based duel index in
    /// the save's 50-slot array. Populated by RebuildDuelGrid, which hides
    /// padding rows — so once padding is hidden, row 0 is no longer duel
    /// array-index 0, and every read/write against the grid needs to go
    /// through this instead of using the row index directly.
    /// </summary>
    private readonly List<int> _duelRowToIndex = new();

    public SaveEditorPage()
    {
        InitializeComponent();
        Paint += BackgroundPainter.Paint;
        RefreshFromState();
    }

    private byte[]? Bytes => AppContext.State?.SaveBytes;
    private LotdSaveVersion Version => AppContext.State?.Version ?? LotdSaveVersion.LinkEvolution;
    private bool HasSave => Bytes is { Length: > 0 };

    /// <summary>Rebuilds every tab's displayed values from the currently loaded
    /// save. Call after a save is (re)loaded and after undo/redo swaps
    /// AppContext.State.SaveBytes.</summary>
    public void RefreshFromState()
    {
        RefreshDuelPointsTab();
        RefreshCampaignTab();
        RefreshUnlocksTab();
        RefreshAvatarsTab();
        RefreshCardsTab();
    }

    private void BeginEdit() => AppContext.Undo.Snapshot();

    private void CommitEdit() => DataChanged?.Invoke(this, EventArgs.Empty);

    // ══════════════════════════════════════════════════════════════════════
    // Duel Points
    // ══════════════════════════════════════════════════════════════════════
    private void RefreshDuelPointsTab()
    {
        bool has = HasSave;
        numDuelPoints.Enabled = has;
        btnSetDuelPoints.Enabled = has;

        if (!has)
        {
            lblDuelPointsCurrent.Text = "Current: —";
            return;
        }

        long current = MiscSaveLayout.GetDuelPoints(Bytes!, Version);
        lblDuelPointsCurrent.Text = $"Current: {current:N0} DP";
        numDuelPoints.Value = Math.Clamp(current, 0L, 999_999_999L);
    }

    private void btnSetDuelPoints_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;
        BeginEdit();
        MiscSaveLayout.SetDuelPoints(Bytes!, Version, (long)numDuelPoints.Value);
        CommitEdit();
        RefreshDuelPointsTab();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Campaign
    // ══════════════════════════════════════════════════════════════════════
    private void RefreshCampaignTab()
    {
        bool has = HasSave;
        cboSeries.Enabled = has;
        btnUnlockSeries.Enabled = has;
        btnCompleteSeries.Enabled = has;
        btnResetSeries.Enabled = has;
        btnUnlockAllCampaign.Enabled = has;
        btnCompleteAllCampaign.Enabled = has;
        btnResetAllCampaign.Enabled = has;
        dgvDuels.Enabled = has;

        _suppressEvents = true;
        try
        {
            cboSeries.Items.Clear();
            if (has)
            {
                var series = CampaignSaveLayout.GetSeries(Version);
                foreach (var s in series) cboSeries.Items.Add(s.ToString());
                if (_selectedSeriesIndex >= series.Length) _selectedSeriesIndex = 0;
                if (cboSeries.Items.Count > 0) cboSeries.SelectedIndex = _selectedSeriesIndex;
            }
        }
        finally
        {
            _suppressEvents = false;
        }

        RebuildDuelGrid();
    }

    private void cboSeries_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressEvents) return;
        _selectedSeriesIndex = Math.Max(0, cboSeries.SelectedIndex);
        RebuildDuelGrid();
    }

    /// <summary>
    /// Confirmed-real duel ranges, per series, within the fixed 50-slot save
    /// array — 0-based array index of the first real duel, and how many real
    /// duels follow it. Everything outside that range (both before and after)
    /// is unused padding and gets hidden from the grid entirely. Counts taken
    /// from the actual campaign duel list (32/32/32/26/33/28 for YuGiOh through
    /// VRAINS) and confirmed in-game: slot #1 (array index 0) of every series
    /// is a padding slot that just has to stay "Available" for the series to
    /// be clickable at all — the real first duel is slot #2 (array index 1).
    /// Series not listed here fall back to showing the full unfiltered 50.
    /// </summary>
    private static readonly Dictionary<LotdDuelSeries, (int RealStart, int RealCount)> KnownRealDuelRange = new()
    {
        { LotdDuelSeries.YuGiOh, (1, 32) },
        { LotdDuelSeries.YuGiOhGX, (1, 32) },
        { LotdDuelSeries.YuGiOh5D, (1, 32) },
        { LotdDuelSeries.YuGiOhZEXAL, (1, 26) },
        { LotdDuelSeries.YuGiOhARCV, (1, 33) },
        { LotdDuelSeries.YuGiOhVRAINS, (1, 28) },
    };

    private void RebuildDuelGrid()
    {
        _suppressEvents = true;
        try
        {
            dgvDuels.Rows.Clear();
            _duelRowToIndex.Clear();
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
                dgvDuels.Rows.Add(i + 1, DisplayState(state), DisplayState(reverse));
                _duelRowToIndex.Add(d);
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    /// <summary>
    /// Maps a raw duel-state value to one of the 5 strings the combo columns
    /// actually offer. Some duel slots (especially never-touched ones, or
    /// duels the currently-selected series doesn't really have) hold raw
    /// int32 values outside the 5 named LotdCampaignDuelState members —
    /// enum.ToString() on those returns the bare number, which isn't in the
    /// combo's Items and makes the DataGridView throw "value is not valid".
    /// Clamp instead of crashing: pixeltris's own notes say anything 4+
    /// behaves like AvailableAlt, so that's the natural bucket for "high"
    /// values, and Locked is the safe bucket for negative/garbage low values.
    /// This is purely a display clamp — writing a cell only ever writes back
    /// one of the 5 named states, so it never corrupts unrelated data.
    /// </summary>
    private static string DisplayState(LotdCampaignDuelState state)
    {
        int raw = (int)state;
        if (raw < 0) return LotdCampaignDuelState.Locked.ToString();
        if (raw > (int)LotdCampaignDuelState.AvailableAlt) return LotdCampaignDuelState.AvailableAlt.ToString();
        return state.ToString();
    }

    private void dgvDuels_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (dgvDuels.IsCurrentCellDirty)
            dgvDuels.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    /// <summary>
    /// Defensive backstop for the DataGridView's own default "value is not
    /// valid" modal — DisplayState above should prevent this from firing at
    /// all, but if some other edge case slips through, swallow it instead of
    /// showing the user a raw .NET exception dialog.
    /// </summary>
    private void dgvDuels_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        e.Cancel = true;
    }

    private void dgvDuels_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (_suppressEvents || !HasSave || e.RowIndex < 0 || e.RowIndex >= _duelRowToIndex.Count) return;

        if (dgvDuels.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is not string text ||
            !Enum.TryParse<LotdCampaignDuelState>(text, out var state))
            return;

        // Padding row #1 (array index 0) is hidden now, so grid row 0 is
        // always the real first duel of the series (array index 1) — it must
        // stay at least Available, or the series becomes unclickable in-game.
        // Matches the same guard the bulk reset buttons apply via
        // CampaignSaveLayout.SetSeries; this covers manually picking "Locked"
        // for that duel's forward state directly in the grid.
        if (e.ColumnIndex == colDuelState.Index && e.RowIndex == 0 && state == LotdCampaignDuelState.Locked)
        {
            state = LotdCampaignDuelState.Available;
            _suppressEvents = true;
            dgvDuels.Rows[0].Cells[colDuelState.Index].Value = DisplayState(state);
            _suppressEvents = false;
        }

        int duelIndex = _duelRowToIndex[e.RowIndex];

        BeginEdit();
        if (e.ColumnIndex == colDuelState.Index)
            CampaignSaveLayout.SetState(Bytes!, Version, _selectedSeriesIndex, duelIndex, state);
        else if (e.ColumnIndex == colDuelReverse.Index)
            CampaignSaveLayout.SetReverseState(Bytes!, Version, _selectedSeriesIndex, duelIndex, state);
        CommitEdit();
    }

    private void btnUnlockSeries_Click(object? sender, EventArgs e) => BulkSetSeries(LotdCampaignDuelState.Available);
    private void btnCompleteSeries_Click(object? sender, EventArgs e) => BulkSetSeries(LotdCampaignDuelState.Complete);
    private void btnResetSeries_Click(object? sender, EventArgs e) => BulkSetSeries(LotdCampaignDuelState.Locked);

    private void BulkSetSeries(LotdCampaignDuelState state)
    {
        if (!HasSave) return;
        BeginEdit();
        // Apply to both directions — forward and reverse duels are tracked
        // separately in the save, so a bulk action that only touched forward
        // would leave every Reverse cell exactly where it was.
        CampaignSaveLayout.SetSeries(Bytes!, Version, _selectedSeriesIndex, state, state);
        CommitEdit();
        RebuildDuelGrid();
    }

    private void btnUnlockAllCampaign_Click(object? sender, EventArgs e) => BulkSetAll(LotdCampaignDuelState.Available);
    private void btnCompleteAllCampaign_Click(object? sender, EventArgs e) => BulkSetAll(LotdCampaignDuelState.Complete);
    private void btnResetAllCampaign_Click(object? sender, EventArgs e) => BulkSetAll(LotdCampaignDuelState.Locked);

    private void BulkSetAll(LotdCampaignDuelState state)
    {
        if (!HasSave) return;
        BeginEdit();
        CampaignSaveLayout.SetAllSeries(Bytes!, Version, state, state);
        CommitEdit();
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
        bool has = HasSave;
        btnUnlockPadlocked.Enabled = has;
        chkContentChallenges.Enabled = has;
        chkContentBattlePack.Enabled = has;
        chkContentCardShop.Enabled = has;
        chkBpEpicDawn.Enabled = has;
        chkBpWotgRound2.Enabled = has;
        clbShopPacks.Enabled = has;
        clbTutorials.Enabled = has;
        btnUnlockChallenges.Enabled = has;
        btnCompleteChallenges.Enabled = has;
        btnResetChallenges.Enabled = has;
        btnUnlockRecipes.Enabled = has;
        btnLockRecipes.Enabled = has;

        _suppressEvents = true;
        try
        {
            clbShopPacks.Items.Clear();
            clbTutorials.Items.Clear();

            if (!has)
            {
                chkContentChallenges.Checked = false;
                chkContentBattlePack.Checked = false;
                chkContentCardShop.Checked = false;
                chkBpEpicDawn.Checked = false;
                chkBpWotgRound2.Checked = false;
                return;
            }

            var content = MiscSaveLayout.GetUnlockedContent(Bytes!, Version);
            chkContentChallenges.Checked = (content & UnlockedContent.DuelistChallenges) != 0;
            chkContentBattlePack.Checked = (content & UnlockedContent.BattlePack) != 0;
            chkContentCardShop.Checked = (content & UnlockedContent.CardShop) != 0;

            var bp = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
            chkBpEpicDawn.Checked = (bp & UnlockedBattlePacks.WarOfTheGiants) != 0;
            chkBpWotgRound2.Checked = (bp & UnlockedBattlePacks.WarOfTheGiantsRound2) != 0;

            var shop = MiscSaveLayout.GetShopPacks(Bytes!, Version);
            foreach (var (flag, label) in ShopPackLabels)
                clbShopPacks.Items.Add(label, (shop & flag) != 0);

            var tut = MiscSaveLayout.GetTutorials(Bytes!, Version);
            for (int i = 1; i <= 20; i++)
            {
                var flag = (CompleteTutorials)(1 << TutorialBitShift(i));
                clbTutorials.Items.Add($"Tutorial {i}", (tut & flag) != 0);
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private static int TutorialBitShift(int tutorialNumber) =>
        tutorialNumber <= 15 ? tutorialNumber : tutorialNumber + 1; // bit 16 is reserved ("Skipped")

    private void btnUnlockPadlocked_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;
        BeginEdit();
        MiscSaveLayout.UnlockPadlockedContent(Bytes!, Version);
        CommitEdit();
        RefreshUnlocksTab();
    }

    private void chkContentChallenges_CheckedChanged(object? sender, EventArgs e) =>
        ToggleUnlockedContent(UnlockedContent.DuelistChallenges, chkContentChallenges.Checked);

    private void chkContentBattlePack_CheckedChanged(object? sender, EventArgs e) =>
        ToggleUnlockedContent(UnlockedContent.BattlePack, chkContentBattlePack.Checked);

    private void chkContentCardShop_CheckedChanged(object? sender, EventArgs e) =>
        ToggleUnlockedContent(UnlockedContent.CardShop, chkContentCardShop.Checked);

    private void ToggleUnlockedContent(UnlockedContent flag, bool on)
    {
        if (_suppressEvents || !HasSave) return;
        BeginEdit();
        var current = MiscSaveLayout.GetUnlockedContent(Bytes!, Version);
        MiscSaveLayout.SetUnlockedContent(Bytes!, Version, on ? current | flag : current & ~flag);
        CommitEdit();
    }

    private void chkBpEpicDawn_CheckedChanged(object? sender, EventArgs e) =>
        ToggleBattlePacks(UnlockedBattlePacks.WarOfTheGiants, chkBpEpicDawn.Checked);

    private void chkBpWotgRound2_CheckedChanged(object? sender, EventArgs e) =>
        ToggleBattlePacks(UnlockedBattlePacks.WarOfTheGiantsRound2, chkBpWotgRound2.Checked);

    private void ToggleBattlePacks(UnlockedBattlePacks flag, bool on)
    {
        if (_suppressEvents || !HasSave) return;
        BeginEdit();
        var current = MiscSaveLayout.GetBattlePacksFlag(Bytes!, Version);
        MiscSaveLayout.SetBattlePacksFlag(Bytes!, Version, on ? current | flag : current & ~flag);
        CommitEdit();
    }

    private void clbShopPacks_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;
        if (e.Index < 0 || e.Index >= ShopPackLabels.Length) return;

        var flag = ShopPackLabels[e.Index].Flag;
        bool on = e.NewValue == CheckState.Checked;

        BeginEdit();
        var current = MiscSaveLayout.GetShopPacks(Bytes!, Version);
        MiscSaveLayout.SetShopPacks(Bytes!, Version, on ? current | flag : current & ~flag);
        CommitEdit();
    }

    private void clbTutorials_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;

        int tutorialNumber = e.Index + 1;
        var flag = (CompleteTutorials)(1 << TutorialBitShift(tutorialNumber));
        bool on = e.NewValue == CheckState.Checked;

        BeginEdit();
        var current = MiscSaveLayout.GetTutorials(Bytes!, Version);
        MiscSaveLayout.SetTutorials(Bytes!, Version, on ? current | flag : current & ~flag);
        CommitEdit();
    }

    private void btnUnlockChallenges_Click(object? sender, EventArgs e) => BulkChallenges(DeulistChallengeState.Available);
    private void btnCompleteChallenges_Click(object? sender, EventArgs e) => BulkChallenges(DeulistChallengeState.Complete);
    private void btnResetChallenges_Click(object? sender, EventArgs e) => BulkChallenges(DeulistChallengeState.Locked);

    private void BulkChallenges(DeulistChallengeState state)
    {
        if (!HasSave) return;
        BeginEdit();
        MiscSaveLayout.SetAllChallenges(Bytes!, Version, state);
        CommitEdit();
    }

    private void btnUnlockRecipes_Click(object? sender, EventArgs e) => BulkRecipes(true);
    private void btnLockRecipes_Click(object? sender, EventArgs e) => BulkRecipes(false);

    private void BulkRecipes(bool unlocked)
    {
        if (!HasSave) return;
        BeginEdit();
        MiscSaveLayout.SetAllRecipesUnlocked(Bytes!, Version, unlocked);
        CommitEdit();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Avatars
    // ══════════════════════════════════════════════════════════════════════
    private void RefreshAvatarsTab()
    {
        bool has = HasSave;
        btnAvatarsAll.Enabled = has;
        btnAvatarsNone.Enabled = has;
        clbAvatars.Enabled = has;

        _suppressEvents = true;
        try
        {
            clbAvatars.Items.Clear();
            if (!has) return;

            for (int i = 0; i < LotdSaveFormat.NumAvatarSlots; i++)
            {
                string name = OwnerDatabase.GetName((byte)i);
                bool unlocked = MiscSaveLayout.GetAvatarUnlocked(Bytes!, Version, i);
                clbAvatars.Items.Add(name, unlocked);
            }
        }
        finally
        {
            _suppressEvents = false;
        }
    }

    private void clbAvatars_ItemCheck(object? sender, ItemCheckEventArgs e)
    {
        if (_suppressEvents || !HasSave) return;

        bool on = e.NewValue == CheckState.Checked;
        BeginEdit();
        MiscSaveLayout.SetAvatarUnlocked(Bytes!, Version, e.Index, on);
        CommitEdit();
    }

    private void btnAvatarsAll_Click(object? sender, EventArgs e) => BulkAvatars(true);
    private void btnAvatarsNone_Click(object? sender, EventArgs e) => BulkAvatars(false);

    private void BulkAvatars(bool unlocked)
    {
        if (!HasSave) return;
        BeginEdit();
        MiscSaveLayout.SetAllAvatarsUnlocked(Bytes!, Version, unlocked);
        CommitEdit();
        RefreshAvatarsTab();
    }

    // ══════════════════════════════════════════════════════════════════════
    // Card Collection (bulk only — see CardCollectionLayout.cs for why)
    // ══════════════════════════════════════════════════════════════════════
    private void RefreshCardsTab()
    {
        bool has = HasSave;
        btnUnlockAllCards.Enabled = has;
        btnMarkAllSeen.Enabled = has;
        numBulkCount.Enabled = has;
        btnApplyCount.Enabled = has;
        btnResetCollection.Enabled = has;

        if (!has)
        {
            lblCardsSummary.Text = "— / — card slots have at least 1 copy owned.";
            return;
        }

        int total = CardCollectionLayout.GetNumCards(Version);
        int owned = 0;
        for (int i = 0; i < total; i++)
            if (CardCollectionLayout.GetCount(Bytes!, Version, i) > 0) owned++;

        lblCardsSummary.Text = $"{owned:N0} / {total:N0} card slots have at least 1 copy owned.";
    }

    private void btnUnlockAllCards_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;
        BeginEdit();
        CardCollectionLayout.UnlockAllCards(Bytes!, Version);
        CommitEdit();
        RefreshCardsTab();
    }

    private void btnMarkAllSeen_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;
        BeginEdit();
        CardCollectionLayout.MarkAllSeen(Bytes!, Version);
        CommitEdit();
        RefreshCardsTab();
    }

    private void btnApplyCount_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;
        BeginEdit();
        CardCollectionLayout.SetAllOwnedCardsCount(Bytes!, Version, (byte)numBulkCount.Value);
        CommitEdit();
        RefreshCardsTab();
    }

    private void btnResetCollection_Click(object? sender, EventArgs e)
    {
        if (!HasSave) return;

        var confirm = AppMessageBox.Show(
            "Reset the entire card collection to 0 copies owned and unseen? This can't be undone from here except with Ctrl+Z.",
            "Reset Card Collection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        BeginEdit();
        CardCollectionLayout.ResetAllCards(Bytes!, Version);
        CommitEdit();
        RefreshCardsTab();
    }
}
