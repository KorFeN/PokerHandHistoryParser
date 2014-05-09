using HandHistories.Statistics.Core;
using HandHistories.Statistics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Statistics
{
    public class BasicHandStatistic : IStatistic
    {
        public static BasicHandStatistic VPIP
        {
            get
            {
                return new BasicHandStatistic(typeof(VPIPOppertunityCondition), typeof(VPIPInstanceCondition), "VPIP");
            }
        }

        public static BasicHandStatistic PFR
        {
            get
            {
                return new BasicHandStatistic(typeof(VPIPOppertunityCondition), typeof(PreflopRaiseCondition), "PFR");
            }
        }

        public static BasicHandStatistic ThreeBet
        {
            get
            {
                return new BasicHandStatistic(typeof(ThreeBetOppertunityCondition), typeof(ThreeBetInstanceCondition), "3Bet");
            }
        }

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
            return (decimal)INST / (decimal)OPP;
        }

        public IEnumerable<Type> Conditions
        {
            get { return new Type[] { _oppertunityCondition, _instanceCondition }; }
        }
    }
}
