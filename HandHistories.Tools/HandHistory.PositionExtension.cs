using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandHistories.Tools;

namespace HandHistories.Tools.PositionExtension
{
    public static class PositionExtension
    {
        public static string GetPlayerInPosition(this HandHistory HH, Street street)
        {
            if (street == Street.Preflop)
            {
                throw new ArgumentException("No player is considered 'In Position', use HandHistory.DealerButtonPosition instead");
            }

            List<HandAction> StreetHAs = HH.HandActions.Street(street).ToList();
            string FirstPlayer = StreetHAs[0].PlayerName;
            for (int i = 1; i < StreetHAs.Count; i++)
            {
                if (StreetHAs[i].PlayerName == FirstPlayer)
                {
                    return StreetHAs[i - 1].PlayerName;
                }
            }
            return StreetHAs[StreetHAs.Count - 1].PlayerName;
        }
    }
}
