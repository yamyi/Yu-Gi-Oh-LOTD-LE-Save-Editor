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

    [JsonIgnore]
    public static Theme Dark => new()
    {
        Name = "Dark",
        IsBuiltIn = true,
        IsDark = true,

        // Backgrounds step up in clear, visible increments (base -> chrome ->
        // card -> hover -> selected) instead of near-identical near-black
        // tones — that flatness was the root of "hard to read, not uniform".
        AppBg = Color.FromArgb(13, 13, 22),
        SidebarBg = Color.FromArgb(22, 22, 34),
        CardBg = Color.FromArgb(28, 28, 42),
        CardHover = Color.FromArgb(40, 40, 60),
        CardSelected = Color.FromArgb(48, 46, 76),
        TopbarBg = Color.FromArgb(22, 22, 34),
        InputBg = Color.FromArgb(24, 24, 36),
        ActionBtnBg = Color.FromArgb(18, 18, 28),

        Border = Color.FromArgb(60, 60, 86),
        BorderHover = Color.FromArgb(96, 96, 160),
        BorderSelected = Color.FromArgb(216, 178, 60),

        TextPrimary = Color.FromArgb(238, 238, 250),
        TextSecondary = Color.FromArgb(190, 190, 212),
        TextMuted = Color.FromArgb(138, 138, 164),
        TextDim = Color.FromArgb(92, 92, 116),
        TextGold = Color.FromArgb(220, 184, 84),
        TextNavActive = Color.FromArgb(220, 184, 84),
        TextNav = Color.FromArgb(146, 146, 172),
        TextInfo = Color.FromArgb(158, 158, 196),
        TextCardName = Color.FromArgb(232, 216, 178),
        TextDescription = Color.FromArgb(198, 192, 176),

        MainGreen = Color.FromArgb(112, 202, 112),
        ExtraBlue = Color.FromArgb(124, 132, 232),
        SideRed = Color.FromArgb(224, 104, 104),
        MainBg = Color.FromArgb(18, 38, 18),
        MainBorder = Color.FromArgb(46, 92, 46),
        ExtraBg = Color.FromArgb(20, 20, 46),
        ExtraBorder = Color.FromArgb(48, 48, 104),
        SideBg = Color.FromArgb(40, 20, 20),
        SideBorder = Color.FromArgb(92, 46, 46),
        StatAtk = Color.FromArgb(228, 106, 106),
        StatDef = Color.FromArgb(104, 170, 232),
        StatMuted = Color.FromArgb(196, 170, 100),
        StatusOk = Color.FromArgb(96, 196, 96),

        GoldBtnBg = Color.FromArgb(216, 178, 60),
        GoldBtnFg = Color.FromArgb(18, 18, 28),
        GoldBtnHover = Color.FromArgb(236, 202, 96),
        ButtonNeutralBg = Color.FromArgb(42, 42, 52),
        ButtonNeutralHover = Color.FromArgb(58, 58, 70),
        DangerFg = Color.FromArgb(230, 118, 118),
        DangerBorder = Color.FromArgb(98, 54, 54),
        DangerHoverBg = Color.FromArgb(56, 28, 28),
        AccentRedHover = Color.FromArgb(206, 68, 68),

        OverlayBackdrop = Color.FromArgb(6, 6, 10),
        PortraitBg = Color.FromArgb(17, 17, 28),
        AccentDivider = Color.FromArgb(70, 46, 112),
        OfflinePanelBg = Color.FromArgb(34, 34, 50),
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
