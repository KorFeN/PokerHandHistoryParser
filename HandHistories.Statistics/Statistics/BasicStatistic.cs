using HandHistories.Statistics.Core;
using HandHistories.Statistics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Conditions.PreFlop;

namespace HandHistories.Statistics.Statistics
{
    public class BasicHandStatistic : IStatistic
    {
        public string Name
        {
            get;
            private set;
        }

        public BasicHandStatistic(Type OppertunityCondition, Type InstanceCondition, string name)
        {
            _oppertunityCondition = OppertunityCondition;
            _instanceCondition = InstanceCondition;
            Name = name;
        }

        Type _oppertunityCondition;
        Type _instanceCondition;

        public decimal GetValue(CounterValueCollection counters)
        {
            int INST = counters[_instanceCondition];
            int OPP = counters[_oppertunityCondition];
            if (OPP == 0)
            {
                return 0;
            }
            return (decimal)INST / (decimal)OPP;
        }

        public IEnumerable<Type> Conditions
        {
            get { return new Type[] { _oppertunityCondition, _instanceCondition }; }
        }
    }
}
