using HandHistories.Objects.Actions;
using HandHistories.Statistics.PlayerStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Core
{
    public delegate void CountTrigger(PrimaryKey key, PlayerHandData player, int CounterID); 

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

        /// <summary>
        /// The Condition this Counter is mapped to.
        /// </summary>
        public Type Condition { get; private set; }

        internal Counter(CounterGroup group, int counterID, Type condition)
        {
            Type typeCheck = condition.GetInterface("IStatisticCondition");
            if (typeCheck == null)
            {
                throw new Exception("Not a statistic");
            }
            Condition = condition;
            ID = counterID;
            coupledGroup = group;
            Name = condition.FullName;
        }

        internal void Initialize(ConditionTree tree)
        {
            tree.GetHandCondition(Condition).ConditionTrigger += CountTrigger;
        }

        private void CountTrigger(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            Count(generalHand.Key, hand, ID);
        }

        public event CountTrigger Count;

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// This class maps indexes in the CounterValueCollection to Counters
    /// </summary>
    public sealed class CounterGroup
    {
        int _currentIndex = 0;
        List<Counter> _counters = new List<Counter>();

        /// <summary>
        /// This class maps indexes in the CounterValueCollection to Counters
        /// </summary>
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

        public CounterValueCollection(CounterGroup statistics)
        {
            _values = new int[statistics.Count];
            _coupledCounters = statistics;
        }

        public CounterValueCollection(CounterGroup statistics, int[] Values)
            :this(statistics)
        {
            _values = Values;
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

        public int[] GetValues()
        {
            return _values.ToArray();
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
