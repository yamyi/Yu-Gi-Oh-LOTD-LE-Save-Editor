using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace YuGiOhSaveEditor.Services;

/// <summary>
/// Maps a card's Attribute ("DARK"/"LIGHT"/etc.) and Race ("Dragon",
/// "Sea Serpent", etc.) to the small icon files bundled under
/// Assets/icons/ - same pack-URI-over-a-plain-Resource approach
/// BanIconConverter already uses for the ban-status corner badges, so no
/// EmbeddedResource/LogicalName plumbing is needed (Assets\icons\*.png is
/// already wildcarded as a plain &lt;Resource&gt; in the csproj).
///
/// Attribute icons (Attribute-DARK.png, etc.) are the official TCG/OCG
/// English icons, sourced as SVG from Yugipedia and rasterized to PNG here.
/// Type icons (Type-Dragon-MADU.png, etc.) are Yu-Gi-Oh! Master Duel's own
/// Type icon set, also from Yugipedia. Both are Konami-owned game assets,
/// not original work - same status as Assets/characters/*.png (see
/// README's Licensing section).
/// </summary>
public static class AttributeTypeIconProvider
{
    /// <summary>Card.Race spells a few Types with a space or hyphen
    /// ("Sea Serpent", "Beast-Warrior") that the Master Duel icon
    /// filenames don't use ("SeaSerpent", "BeastWarrior") - this is the
    /// full set of exceptions; every other Race matches its filename
    /// verbatim (e.g. "Dragon" -> Type-Dragon-MADU.png).</summary>
    private static readonly Dictionary<string, string> RaceToFileKey = new()
    {
        ["Beast-Warrior"] = "BeastWarrior",
        ["Creator God"] = "CreatorGod",
        ["Divine-Beast"] = "DivineBeast",
        ["Sea Serpent"] = "SeaSerpent",
        ["Winged Beast"] = "WingedBeast",
    };

    /// <summary>Null for Spell/Trap cards (no Attribute) or an Attribute
    /// this icon set doesn't have a file for - callers just leave the
    /// Image's Source unset/null in that case, same as every other
    /// optional preview element here.</summary>
    public static BitmapImage? AttributeIcon(string? attribute)
    {
        if (string.IsNullOrEmpty(attribute)) return null;
        return LoadPackedIcon($"Attribute-{attribute}.png");
    }

    /// <summary>Null for non-monsters (no Race) or a Race this icon set
    /// doesn't have a file for.</summary>
    public static BitmapImage? TypeIcon(string? race)
    {
        if (string.IsNullOrEmpty(race)) return null;
        string fileKey = RaceToFileKey.TryGetValue(race, out var mapped) ? mapped : race;
        return LoadPackedIcon($"Type-{fileKey}-MADU.png");
    }

    /// <summary>The big round "SPELL"/"TRAP" badge (Card.Type is literally
    /// "Spell Card" / "Trap Card") - null for monsters, which use
    /// TypeIcon/AttributeIcon instead.</summary>
    public static BitmapImage? CardTypeIcon(string? type)
    {
        if (string.IsNullOrEmpty(type)) return null;
        if (type.Contains("Spell", StringComparison.OrdinalIgnoreCase)) return LoadPackedIcon("Spell.png");
        if (type.Contains("Trap", StringComparison.OrdinalIgnoreCase)) return LoadPackedIcon("Trap.png");
        return null;
    }

    /// <summary>Card.HumanReadableCardType spells the Quick-Play subtype
    /// with a hyphen ("Quick-Play Spell") but the icon file doesn't
    /// ("Subtype-QuickPlay.png") - this is the one exception; every other
    /// subtype (Normal/Continuous/Equip/Field/Ritual/Counter) matches its
    /// filename verbatim once " Spell"/" Trap" is stripped off.</summary>
    private static readonly Dictionary<string, string> SubTypeToFileKey = new()
    {
        ["Quick-Play"] = "QuickPlay",
    };

    /// <summary>The small subtype icon (Normal/Continuous/Equip/Field/
    /// Quick-Play/Ritual/Counter) shown next to a Spell/Trap's full
    /// subtype name - null for monsters.</summary>
    public static BitmapImage? SubTypeIcon(string? humanReadableCardType)
    {
        if (string.IsNullOrEmpty(humanReadableCardType)) return null;
        string subtype = humanReadableCardType
            .Replace(" Spell", "", StringComparison.OrdinalIgnoreCase)
            .Replace(" Trap", "", StringComparison.OrdinalIgnoreCase);
        string fileKey = SubTypeToFileKey.TryGetValue(subtype, out var mapped) ? mapped : subtype;
        return LoadPackedIcon($"Subtype-{fileKey}.png");
    }

    private static BitmapImage? LoadPackedIcon(string fileName)
    {
        try
        {
            return new BitmapImage(new Uri($"pack://application:,,,/Assets/icons/{fileName}"));
        }
        catch (IOException)
        {
            // File genuinely isn't in this icon set (e.g. a Race/Attribute
            // Cards.json has that Yugipedia didn't have a matching icon
            // for) - same "just omit it" behavior as every other optional
            // preview field, rather than letting a missing-icon corner
            // case crash the preview.
            return null;
        }
    }
}
