using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Flop
{
    public class FlopFoldOppertunityCondition : FoldOppertunity
    {
        public FlopFoldOppertunityCondition()
            : base(Street.Flop)
        {
        }
    }

    public class FlopFoldInstanceCondition : FoldInstance
    {
        public FlopFoldInstanceCondition()
            : base(Street.Flop)
        {
        }
    }
}
