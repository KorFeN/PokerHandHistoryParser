using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.River
{
    public class RiverContinuationBetOppertunityCondition : ContinuationBetOppertunity
    {
        public RiverContinuationBetOppertunityCondition()
            : base(Street.River)
        {
        }
    }

    public class RiverContinuationBetInstanceCondition : ContinuationBetInstance
    {
        public RiverContinuationBetInstanceCondition()
            : base(Street.River)
        {
        }
    }
}
