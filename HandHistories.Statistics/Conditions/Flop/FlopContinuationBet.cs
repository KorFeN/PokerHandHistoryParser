using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Flop
{
    public class FlopContinuationBetOppertunityCondition : ContinuationBetOppertunity
    {
        public FlopContinuationBetOppertunityCondition()
            : base(Street.Flop)
        {
        }
    }

    public class FlopContinuationBetInstanceCondition : ContinuationBetInstance
    {
        public FlopContinuationBetInstanceCondition()
            : base(Street.Flop)
        {
        }
    }
}
