using HandHistories.Statistics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    public interface IStatistic
    {
        IEnumerable<IStatisticCondition> Conditions { get; }
        void Initialize(ConditionTree tree);
        string Name { get; }
        decimal Value { get; }
    }
}
