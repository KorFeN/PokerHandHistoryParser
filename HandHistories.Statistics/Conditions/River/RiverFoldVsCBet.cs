using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.River
{
    public class RiverFoldVsCBetOppertunityCondition : FoldVsCBetOppertunity
    {
        public RiverFoldVsCBetOppertunityCondition()
            : base(Street.River)
        {
        }
    }

    public class RiverFoldVsCBetInstanceCondition : FoldVsCBetInstance
    {
        public RiverFoldVsCBetInstanceCondition()
            : base(Street.River)
        {
        }
    }
}