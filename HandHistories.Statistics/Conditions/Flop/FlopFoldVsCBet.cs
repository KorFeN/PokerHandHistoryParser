using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.Flop
{
    public class FlopFoldVsCBetOppertunityCondition : FoldVsCBetOppertunity
    {
        public FlopFoldVsCBetOppertunityCondition()
            : base(Street.Flop)
        {
        }
    }

    public class FlopFoldVsCBetInstanceCondition : FoldVsCBetInstance
    {
        public FlopFoldVsCBetInstanceCondition()
            : base(Street.Flop)
        {
        }
    }
}
