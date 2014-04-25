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
        public static BasicHandStatistic CreateVPIPStatistic()
        {
            return new BasicHandStatistic(new VPIPOppertunityCondition(), new VPIPInstanceCondition(), "VPIP");
        }

        public static BasicHandStatistic CreatePFRStatistic()
        {
            return new BasicHandStatistic(new VPIPOppertunityCondition(), new PreflopRaiseCondition(), "PFR");
        }

        public static BasicHandStatistic Create3BetStatistic()
        {
            return new BasicHandStatistic(new ThreeBetOppertunityCondition(), new ThreeBetInstanceCondition(), "3Bet");
        }

        public string Name
        {
            get;
            private set;
        }

        public BasicHandStatistic(IStatisticCondition OppertunityCondition, IStatisticCondition InstanceCondition, string name)
        {
            _oppertunityCondition = OppertunityCondition;
            _instanceCondition = InstanceCondition;
            Name = name;
        }

        IStatisticCondition _oppertunityCondition;
        IStatisticCondition _instanceCondition;
        SimpleStatCounter OppertunityCounter;
        SimpleStatCounter InstanceCounter;

        public decimal Value
        {
            get { return (decimal)InstanceCounter.Count / (decimal)OppertunityCounter.Count; }
        }

        public void Initialize(ConditionTree tree)
        {
            tree.InitializationFinnished -= Initialize;
            OppertunityCounter = new SimpleStatCounter(tree.GetHandCondition(_oppertunityCondition.GetType()));
            InstanceCounter = new SimpleStatCounter(tree.GetHandCondition(_instanceCondition.GetType()));
        }

        public IEnumerable<IStatisticCondition> Conditions
        {
            get { return new IStatisticCondition[] { _oppertunityCondition, _instanceCondition }; }
        }
    }
}
