using HandHistories.Objects.Cards;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.River
{
    public class RiverBetOppertunityCondition : BetOppertunity
    {
        public RiverBetOppertunityCondition()
            : base(Street.River)
        {
        }
    }

    public class RiverBetInstanceCondition : BetInstance
    {
        public RiverBetInstanceCondition()
            : base(Street.River)
        {
        }
    }
}
