using HandHistories.Objects.Actions;
using HandHistories.Statistics.PlayerStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Core
{
    public sealed class Counter
    {
        /// <summary>
        /// The counter group of which this counter is a member
        /// </summary>
        readonly CounterGroup coupledGroup;
        /// <summary>
        /// Index where this counter is located in the CounterGroup
        /// </summary>
        public int ID { get; private set; }
        public string Name { get; private set; }
        public Type Condition { get; private set; }
        

        internal Counter(CounterGroup group, int pID, Type condition)
        {
            Type typeCheck = condition.GetInterface("IStatisticCondition");
            if (typeCheck == null)
            {
                throw new Exception("Not a statistic");
            }
            Condition = condition;
            ID = pID;
            coupledGroup = group;
            Name = condition.FullName;
        }

        internal void Initialize(ConditionTree tree)
        {
            tree.GetHandCondition(Condition).ConditionTrigger += CountTrigger;
        }

        private void CountTrigger(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            Count(hand.playerName, generalHand.Key, ID);
        }

        public event Action<string, PrimaryKey, int> Count;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// This class maps indexes in the to counters
    /// </summary>
    public sealed class CounterGroup
    {
        int _currentIndex = 0;
        List<Counter> _counters = new List<Counter>();

        internal CounterGroup()
        {
        }

        public int Count { get { return _counters.Count; } }

        public Counter this[int ID]
        {
            get
            {
                return _counters[ID];
            }
        }

        public Counter this[Type ID]
        {
            get
            {
                return _counters.Find(p => p.Condition == ID);
            }
        }

        public Counter CreateCounter(Type condition)
        {
            int counterIndex = _counters.FindIndex(p => p.Name == condition.FullName);
            if (counterIndex == -1)
            {
                Counter newCounter = new Counter(this, _currentIndex++, condition);
                _counters.Add(newCounter);
                return newCounter;
            }
            else
            {
                return null;
            }
        }

        public void Initialize(ConditionTree tree)
        {
            foreach (var counter in _counters)
            {
                counter.Initialize(tree);
            }
        }

        public override string ToString()
        {
            return "CounterGroup: " + _counters.Count + " counters.";
        }
    }

    /// <summary>
    /// The CounterValueCollection is a collection of integers coupled to a CounterGroup
    /// </summary>
    public sealed class CounterValueCollection
    {
        readonly CounterGroup _coupledCounters;
        int[] _values;

        internal CounterValueCollection(CounterGroup statistics)
        {
            _values = new int[statistics.Count];
            _coupledCounters = statistics;
        }

        internal CounterValueCollection(CounterGroup statistics, int[] Values)
            :this(statistics)
        {
        }

        public int this[Type condition]
        {
            get
            {
                return _values[_coupledCounters[condition].ID];
            }
        }

        public void IncrementCounter(int counterID)
        {
            _values[counterID]++;
        }

        public static CounterValueCollection operator +(CounterValueCollection op1, CounterValueCollection op2)
        {
            if (op1._coupledCounters != op2._coupledCounters)
            {
                throw new ArgumentException("Diffrent Collections are added.");
            }
            var counters = new CounterValueCollection(op1._coupledCounters);
            for (int i = 0; i < counters._values.Length; i++)
			{
                counters._values[i] = op1._values[i] + op2._values[i];
			}
            return counters;
        }

        public override string ToString()
        {
            return "CounterValueCollection: " + _coupledCounters.Count;
        }
    }
}
