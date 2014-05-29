using HandHistories.Objects.Cards;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.Turn
{
    public class TurnBetOppertunityCondition : BetOppertunity
    {
        public TurnBetOppertunityCondition()
            : base(Street.Turn)
        {
        }
    }

    public class TurnBetInstanceCondition : BetInstance
    {
        public TurnBetInstanceCondition()
            : base(Street.Turn)
        {
        }
    }
}
