using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Turn
{
    public class TurnContinuationBetOppertunityCondition : ContinuationBetOppertunity
    {
        public TurnContinuationBetOppertunityCondition()
            : base(Street.Turn)
        {
        }
    }

    public class TurnContinuationBetInstanceCondition : ContinuationBetInstance
    {
        public TurnContinuationBetInstanceCondition()
            : base(Street.Turn)
        {
        }
    }
}
