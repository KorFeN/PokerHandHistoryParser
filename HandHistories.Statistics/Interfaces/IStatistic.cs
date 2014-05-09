using HandHistories.Statistics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    public interface IStatistic
    {
        string Name { get; }
        IEnumerable<Type> Conditions { get; }
        decimal GetValue(CounterValueCollection valueCollection);
    }
}
