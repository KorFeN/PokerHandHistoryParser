using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    public class SimpleStatCounter : IStatisticCounter
    {
        public readonly IStatisticCondition Condition;

        public SimpleStatCounter(IStatisticCondition CountCondition)
        {
            Condition = CountCondition;
            CountCondition.ConditionTrigger += delegate { Count++; };
        }

        public int Count
        {
            get;
            private set;
        }
    }
}
