namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// Every color the app draws with, named by role. These used to be static
/// readonly fields holding one hardcoded dark-navy/gold palette; they're now
/// properties that read through to ThemeManager.Current, so every existing
/// "AppColors.X" call site (Designer.cs files included, after the theming
/// sweep) automatically follows whatever theme is active — built-in Dark,
/// built-in Light, or a user's custom theme — with zero changes needed at
/// the call sites themselves.
/// </summary>
public static class AppColors
{
    // ── Backgrounds ──────────────────────────────────────────────────────────
    public static Color AppBg         => ThemeManager.Current.AppBg;
    public static Color SidebarBg     => ThemeManager.Current.SidebarBg;
    public static Color CardBg        => ThemeManager.Current.CardBg;
    public static Color CardHover     => ThemeManager.Current.CardHover;
    public static Color CardSelected  => ThemeManager.Current.CardSelected;
    public static Color TopbarBg      => ThemeManager.Current.TopbarBg;
    public static Color InputBg       => ThemeManager.Current.InputBg;
    public static Color ActionBtnBg   => ThemeManager.Current.ActionBtnBg;

    // ── Borders ───────────────────────────────────────────────────────────────
    public static Color Border         => ThemeManager.Current.Border;
    public static Color BorderHover    => ThemeManager.Current.BorderHover;
    public static Color BorderSelected => ThemeManager.Current.BorderSelected;

    // ── Text ──────────────────────────────────────────────────────────────────
    public static Color TextPrimary    => ThemeManager.Current.TextPrimary;
    public static Color TextSecondary  => ThemeManager.Current.TextSecondary;
    public static Color TextMuted      => ThemeManager.Current.TextMuted;
    public static Color TextDim        => ThemeManager.Current.TextDim;
    public static Color TextGold       => ThemeManager.Current.TextGold;
    public static Color TextNavActive  => ThemeManager.Current.TextNavActive;
    public static Color TextNav        => ThemeManager.Current.TextNav;
    public static Color TextInfo       => ThemeManager.Current.TextInfo;
    public static Color TextCardName   => ThemeManager.Current.TextCardName;
    public static Color TextDescription=> ThemeManager.Current.TextDescription;

    // ── Pip / stat colours ────────────────────────────────────────────────────
    public static Color MainGreen   => ThemeManager.Current.MainGreen;
    public static Color ExtraBlue   => ThemeManager.Current.ExtraBlue;
    public static Color SideRed     => ThemeManager.Current.SideRed;
    public static Color MainBg      => ThemeManager.Current.MainBg;
    public static Color MainBorder  => ThemeManager.Current.MainBorder;
    public static Color ExtraBg     => ThemeManager.Current.ExtraBg;
    public static Color ExtraBorder => ThemeManager.Current.ExtraBorder;
    public static Color SideBg      => ThemeManager.Current.SideBg;
    public static Color SideBorder  => ThemeManager.Current.SideBorder;
    public static Color StatAtk     => ThemeManager.Current.StatAtk;
    public static Color StatDef     => ThemeManager.Current.StatDef;
    public static Color StatMuted   => ThemeManager.Current.StatMuted;
    public static Color StatusOk    => ThemeManager.Current.StatusOk;

    // ── Buttons ───────────────────────────────────────────────────────────────
    public static Color GoldBtnBg         => ThemeManager.Current.GoldBtnBg;
    public static Color GoldBtnFg         => ThemeManager.Current.GoldBtnFg;
    public static Color GoldBtnHover      => ThemeManager.Current.GoldBtnHover;
    public static Color ButtonNeutralBg   => ThemeManager.Current.ButtonNeutralBg;
    public static Color ButtonNeutralHover=> ThemeManager.Current.ButtonNeutralHover;
    public static Color DangerFg          => ThemeManager.Current.DangerFg;
    public static Color DangerBorder      => ThemeManager.Current.DangerBorder;
    public static Color DangerHoverBg     => ThemeManager.Current.DangerHoverBg;
    public static Color AccentRedHover    => ThemeManager.Current.AccentRedHover;

    // ── Misc ──────────────────────────────────────────────────────────────────
    public static Color OverlayBackdrop => ThemeManager.Current.OverlayBackdrop;
    public static Color PortraitBg      => ThemeManager.Current.PortraitBg;
    public static Color AccentDivider   => ThemeManager.Current.AccentDivider;
    public static Color OfflinePanelBg  => ThemeManager.Current.OfflinePanelBg;
}
