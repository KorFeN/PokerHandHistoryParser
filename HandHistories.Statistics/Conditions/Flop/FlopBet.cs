using HandHistories.Objects.Cards;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.Flop
{
    public class FlopBetOppertunityCondition : BetOppertunity
    {
        public FlopBetOppertunityCondition()
            : base(Street.Flop)
        {
        }
    }

    public class FlopBetInstanceCondition : BetInstance
    {
        public FlopBetInstanceCondition()
            : base(Street.Flop)
        {
        }
    }
}
