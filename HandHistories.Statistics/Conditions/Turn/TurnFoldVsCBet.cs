using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Turn
{
    public class TurnFoldVsCBetOppertunityCondition : FoldVsCBetOppertunity
    {
        public TurnFoldVsCBetOppertunityCondition()
            : base(Street.Turn)
        {
        }
    }

    public class TurnFoldVsCBetInstanceCondition : FoldVsCBetInstance
    {
        public TurnFoldVsCBetInstanceCondition()
            : base(Street.Turn)
        {
        }
    }
}