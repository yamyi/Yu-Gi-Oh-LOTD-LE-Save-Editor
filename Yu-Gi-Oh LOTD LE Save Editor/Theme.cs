using System.Text.Json.Serialization;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// A full color palette plus an optional background image. Every color the
/// app draws with is a named role here, so switching/customizing a theme
/// recolors the whole app consistently — no scattered literals.
/// Built-in themes (Dark/Light) are marked IsBuiltIn; customizing one clones
/// it into a new named custom theme instead of editing it in place.
/// Persisted as JSON by ThemeManager.
/// </summary>
public sealed class Theme
{
    public string Name { get; set; } = "Custom";
    public bool IsBuiltIn { get; set; }
    public bool IsDark { get; set; } = true;

    // ── Backgrounds ──────────────────────────────────────────────────────────
    public Color AppBg { get; set; }
    public Color SidebarBg { get; set; }
    public Color CardBg { get; set; }
    public Color CardHover { get; set; }
    public Color CardSelected { get; set; }
    public Color TopbarBg { get; set; }
    public Color InputBg { get; set; }
    public Color ActionBtnBg { get; set; }

    // ── Borders ───────────────────────────────────────────────────────────────
    public Color Border { get; set; }
    public Color BorderHover { get; set; }
    public Color BorderSelected { get; set; }

    // ── Text ──────────────────────────────────────────────────────────────────
    public Color TextPrimary { get; set; }
    public Color TextSecondary { get; set; }
    public Color TextMuted { get; set; }
    public Color TextDim { get; set; }
    public Color TextGold { get; set; }
    public Color TextNavActive { get; set; }
    public Color TextNav { get; set; }
    public Color TextInfo { get; set; }
    public Color TextCardName { get; set; }
    public Color TextDescription { get; set; }

    // ── Pip / stat colours ────────────────────────────────────────────────────
    public Color MainGreen { get; set; }
    public Color ExtraBlue { get; set; }
    public Color SideRed { get; set; }
    public Color MainBg { get; set; }
    public Color MainBorder { get; set; }
    public Color ExtraBg { get; set; }
    public Color ExtraBorder { get; set; }
    public Color SideBg { get; set; }
    public Color SideBorder { get; set; }
    public Color StatAtk { get; set; }
    public Color StatDef { get; set; }
    public Color StatMuted { get; set; }
    public Color StatusOk { get; set; }

    // ── Buttons ───────────────────────────────────────────────────────────────
    public Color GoldBtnBg { get; set; }
    public Color GoldBtnFg { get; set; }
    public Color GoldBtnHover { get; set; }
    public Color ButtonNeutralBg { get; set; }
    public Color ButtonNeutralHover { get; set; }
    public Color DangerFg { get; set; }
    public Color DangerBorder { get; set; }
    public Color DangerHoverBg { get; set; }
    public Color AccentRedHover { get; set; }

    // ── Misc ──────────────────────────────────────────────────────────────────
    public Color OverlayBackdrop { get; set; }
    public Color PortraitBg { get; set; }
    public Color AccentDivider { get; set; }
    public Color OfflinePanelBg { get; set; }

    // ── Background image (the "windows background" feature) ────────────────────
    // Painted behind MainForm's chrome, dimmed by BackgroundOverlayOpacity so
    // text stays readable. Solid content panels (card grids, editors) keep
    // their own theme color on top and won't show the image through them —
    // WinForms doesn't support real per-pixel transparency for child controls.
    public string? BackgroundImagePath { get; set; }
    public double BackgroundOverlayOpacity { get; set; } = 0.55;

    /// <summary>Deep-enough copy (Color/string/double are all copied by value)
    /// under a new name, for "customize this preset" flows.</summary>
    public Theme Clone(string newName)
    {
        var clone = (Theme)MemberwiseClone();
        clone.Name = newName;
        clone.IsBuiltIn = false;
        return clone;
    }

    /// <summary>
    /// The app's one fixed palette — a "Yu-Gi-Oh! Duel Links" mobile-UI look:
    /// deep navy backgrounds, bright cyan borders/dividers, a teal gradient for
    /// primary CTA buttons (matches Duel Links' "Card Inventory"/"Information"
    /// buttons), a blue gradient for secondary buttons ("MENU"/"Auto-Build
    /// Deck"/"Copy Deck"), gold for callouts and active-tab state, and red for
    /// warnings — all traced from actual Duel Links screenshots (deck editor,
    /// deck list, Duel Studio menu, main hub, Settings modal, Help/Etc. list).
    /// Kept under the name "Dark" / IsBuiltIn so ThemeManager's existing
    /// load/persist/property-role plumbing didn't need touching — this app
    /// now only ships the one look (see MainForm's removed theme switcher).
    /// </summary>
    [JsonIgnore]
    public static Theme Dark => new()
    {
        Name = "Dark",
        IsBuiltIn = true,
        IsDark = true,

        // Backgrounds: near-black navy canvas -> navy chrome (top/bottom bars)
        // -> navy panel (info boxes, cards) -> lighter navy hover -> cyan-
        // tinted selected, matching the deck-editor's info-panel/slot styling.
        AppBg = Color.FromArgb(7, 12, 22),
        SidebarBg = Color.FromArgb(9, 18, 32),
        CardBg = Color.FromArgb(13, 28, 50),
        CardHover = Color.FromArgb(19, 40, 70),
        CardSelected = Color.FromArgb(22, 56, 88),
        TopbarBg = Color.FromArgb(7, 11, 20),
        InputBg = Color.FromArgb(11, 24, 44),
        ActionBtnBg = Color.FromArgb(14, 52, 98),

        // Borders: the bright cyan outline used on every panel/slot/divider.
        Border = Color.FromArgb(42, 188, 220),
        BorderHover = Color.FromArgb(108, 228, 250),
        BorderSelected = Color.FromArgb(247, 199, 64),

        TextPrimary = Color.FromArgb(240, 246, 250),
        TextSecondary = Color.FromArgb(178, 206, 224),
        TextMuted = Color.FromArgb(116, 146, 176),
        TextDim = Color.FromArgb(78, 100, 130),
        TextGold = Color.FromArgb(250, 202, 60),
        TextNavActive = Color.FromArgb(250, 202, 60),
        TextNav = Color.FromArgb(140, 190, 214),
        TextInfo = Color.FromArgb(118, 200, 230),
        TextCardName = Color.FromArgb(235, 214, 168),
        TextDescription = Color.FromArgb(200, 214, 226),

        // Main = cyan, Extra = magenta, Side = red — matches the Main/Extra
        // deck slot border colors in the Duel Links deck editor screenshot.
        MainGreen = Color.FromArgb(70, 210, 235),
        ExtraBlue = Color.FromArgb(224, 90, 190),
        SideRed = Color.FromArgb(230, 110, 110),
        MainBg = Color.FromArgb(9, 32, 46),
        MainBorder = Color.FromArgb(50, 195, 225),
        ExtraBg = Color.FromArgb(36, 12, 34),
        ExtraBorder = Color.FromArgb(210, 70, 170),
        SideBg = Color.FromArgb(40, 14, 14),
        SideBorder = Color.FromArgb(200, 80, 80),
        StatAtk = Color.FromArgb(235, 120, 90),
        StatDef = Color.FromArgb(110, 180, 235),
        StatMuted = Color.FromArgb(190, 175, 120),
        StatusOk = Color.FromArgb(90, 210, 120),

        // "Gold" button role repurposed as the teal primary-CTA color (Duel
        // Links reserves actual gold for small accents/active tabs, not
        // buttons — its primary action buttons are teal/cyan).
        GoldBtnBg = Color.FromArgb(18, 150, 165),
        GoldBtnFg = Color.FromArgb(8, 20, 30),
        GoldBtnHover = Color.FromArgb(40, 185, 200),
        ButtonNeutralBg = Color.FromArgb(18, 70, 130),
        ButtonNeutralHover = Color.FromArgb(28, 95, 165),
        DangerFg = Color.FromArgb(235, 120, 120),
        DangerBorder = Color.FromArgb(150, 60, 60),
        DangerHoverBg = Color.FromArgb(58, 20, 20),
        AccentRedHover = Color.FromArgb(220, 80, 80),

        OverlayBackdrop = Color.FromArgb(4, 8, 14),
        PortraitBg = Color.FromArgb(10, 22, 40),
        AccentDivider = Color.FromArgb(40, 130, 160),
        OfflinePanelBg = Color.FromArgb(15, 33, 57),
    };

    [JsonIgnore]
    public static Theme Light => new()
    {
        Name = "Light",
        IsBuiltIn = true,
        IsDark = false,

        // Cards sit as elevated white surfaces on a soft grey canvas, with
        // clearly distinct hover/selected steps (not 5-10 units apart) and
        // dark-on-light text with real contrast at every tier.
        AppBg = Color.FromArgb(236, 237, 242),
        SidebarBg = Color.FromArgb(255, 255, 255),
        CardBg = Color.FromArgb(255, 255, 255),
        CardHover = Color.FromArgb(230, 233, 244),
        CardSelected = Color.FromArgb(212, 219, 245),
        TopbarBg = Color.FromArgb(255, 255, 255),
        InputBg = Color.FromArgb(255, 255, 255),
        ActionBtnBg = Color.FromArgb(236, 237, 242),

        Border = Color.FromArgb(206, 208, 222),
        BorderHover = Color.FromArgb(150, 160, 214),
        BorderSelected = Color.FromArgb(170, 128, 20),

        TextPrimary = Color.FromArgb(18, 18, 30),
        TextSecondary = Color.FromArgb(52, 52, 74),
        TextMuted = Color.FromArgb(112, 112, 136),
        TextDim = Color.FromArgb(160, 160, 182),
        TextGold = Color.FromArgb(142, 102, 8),
        TextNavActive = Color.FromArgb(142, 102, 8),
        TextNav = Color.FromArgb(96, 96, 122),
        TextInfo = Color.FromArgb(80, 84, 130),
        TextCardName = Color.FromArgb(96, 74, 20),
        TextDescription = Color.FromArgb(68, 64, 54),

        MainGreen = Color.FromArgb(24, 116, 30),
        ExtraBlue = Color.FromArgb(44, 64, 172),
        SideRed = Color.FromArgb(180, 34, 34),
        MainBg = Color.FromArgb(220, 240, 220),
        MainBorder = Color.FromArgb(140, 195, 140),
        ExtraBg = Color.FromArgb(222, 222, 246),
        ExtraBorder = Color.FromArgb(140, 140, 205),
        SideBg = Color.FromArgb(246, 222, 222),
        SideBorder = Color.FromArgb(205, 140, 140),
        StatAtk = Color.FromArgb(170, 32, 32),
        StatDef = Color.FromArgb(20, 82, 160),
        StatMuted = Color.FromArgb(124, 94, 20),
        StatusOk = Color.FromArgb(18, 104, 18),

        GoldBtnBg = Color.FromArgb(201, 162, 39),
        GoldBtnFg = Color.FromArgb(24, 24, 34),
        GoldBtnHover = Color.FromArgb(176, 138, 24),
        ButtonNeutralBg = Color.FromArgb(226, 227, 234),
        ButtonNeutralHover = Color.FromArgb(208, 210, 222),
        DangerFg = Color.FromArgb(170, 48, 48),
        DangerBorder = Color.FromArgb(220, 180, 180),
        DangerHoverBg = Color.FromArgb(248, 218, 218),
        AccentRedHover = Color.FromArgb(182, 46, 46),

        OverlayBackdrop = Color.FromArgb(6, 6, 10),
        PortraitBg = Color.FromArgb(222, 223, 232),
        AccentDivider = Color.FromArgb(198, 186, 220),
        OfflinePanelBg = Color.FromArgb(230, 231, 240),
    };
}
