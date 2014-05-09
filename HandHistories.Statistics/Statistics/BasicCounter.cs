using HandHistories.Statistics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions;

namespace HandHistories.Statistics.Statistics
{
    public class BasicCounterStatistic : IStatistic
    {
        public static BasicCounterStatistic Hands { get { return new BasicCounterStatistic(typeof(PlayerHandCondition), "Hands"); } }

        public string Name
        {
            get;
            private set;
        }

        public BasicCounterStatistic(Type InstanceCondition, string name)
        {
            _instanceCondition = InstanceCondition;
            Name = name;
        }

        Type _instanceCondition;

        public decimal GetValue(CounterValueCollection counters)
        {
            return (decimal)counters[_instanceCondition];
        }

        public IEnumerable<Type> Conditions
        {
            get { return new Type[] {_instanceCondition }; }
        }
    }
}
