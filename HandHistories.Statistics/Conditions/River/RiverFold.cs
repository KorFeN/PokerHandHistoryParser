using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.StreetBaseConditions;
using HandHistories.Objects.Cards;

namespace HandHistories.Statistics.Conditions.River
{
    public class RiverFoldOppertunityCondition : FoldOppertunity
    {
        public RiverFoldOppertunityCondition()
            : base(Street.River)
        {
        }
    }

    public class RiverFoldInstanceCondition : FoldInstance
    {
        public RiverFoldInstanceCondition()
            : base(Street.River)
        {
        }
    }
}
