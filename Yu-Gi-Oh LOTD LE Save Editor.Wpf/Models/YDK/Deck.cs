using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuGiOhSaveEditor.Wpf.Services
{
    public class Deck
    {
        public List<Card> main=new List<Card>();
        public List<Card> extra = new List<Card>();
        public List<Card> side = new List<Card>();

        public Deck()
        {
        }

        public Deck(List<Card> main, List<Card> extra, List<Card> side)
        {
            this.main = main;
            this.extra = extra;
            this.side = side;
        }

        public ushort[] Get_Lotd_Main()
        {
            List<ushort> lotd_main = new List<ushort>();
            foreach (var card in main)
            {
                if (card != null)
                {
                    lotd_main.Add((ushort)card.LotdId);
                }
            }
            return lotd_main.ToArray();
        }

        public ushort[] Get_Lotd_Extra()
        {
            List<ushort> lotd_extra = new List<ushort>();
            foreach (var card in extra)
            {
                if (card != null)
                {
                    lotd_extra.Add((ushort)card.LotdId);
                }
            }
            return lotd_extra.ToArray();
        }

        public ushort[] Get_Lotd_Side()
        {
            List<ushort> lotd_side = new List<ushort>();
            foreach (var card in side)
            {
                if (card != null)
                {
                    lotd_side.Add((ushort)card.LotdId);
                }
            }
            return lotd_side.ToArray();
        }
    }
}
