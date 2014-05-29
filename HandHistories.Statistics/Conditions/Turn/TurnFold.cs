using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Turn
{
    public class TurnFoldOppertunityCondition : FoldOppertunity
    {
        public TurnFoldOppertunityCondition()
            : base(Street.Turn)
        {
        }
    }

    public class TurnFoldInstanceCondition : FoldInstance
    {
        public TurnFoldInstanceCondition()
            : base(Street.Turn)
        {
        }
    }
}