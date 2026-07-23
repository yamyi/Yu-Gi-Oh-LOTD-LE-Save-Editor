using System.IO;
using System.Text;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Writes a Deck out as a standard .ydk file (passcodes under #main / #extra / !side),
    /// the counterpart to YDKReader.Parse.
    /// </summary>
    public static class YDKWriter
    {
        public static void Write(string path, Deck deck)
        {
            var sb = new StringBuilder();

            sb.AppendLine("#created by Yu-Gi-Oh LOTD:LE Deck Editor");
            sb.AppendLine("#main");
            foreach (var card in deck.main)
                if (card is not null) sb.AppendLine(card.Id.ToString());

            sb.AppendLine("#extra");
            foreach (var card in deck.extra)
                if (card is not null) sb.AppendLine(card.Id.ToString());

            sb.AppendLine("!side");
            foreach (var card in deck.side)
                if (card is not null) sb.AppendLine(card.Id.ToString());

            File.WriteAllText(path, sb.ToString());
        }
    }
}
