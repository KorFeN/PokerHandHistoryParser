using HandHistories.Objects.GameDescription;
using HandHistories.Statistics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.PlayerStats
{
    public class PlayerStatistics : IEnumerable<KeyValuePair<PrimaryKey, CounterValueCollection>>
    {
        public SiteName Site { get; set; }
        public string Name { get; set; }

        CounterGroup counterGroup;
        public CounterGroup CounterGroup
        {
            get
            {
                return counterGroup;
            }
            set
            {
                if (counterGroup != null)
                {
                    throw new Exception();
                }
                counterGroup = value;
            }
        }

        public Dictionary<PrimaryKey, CounterValueCollection> counters;

        public PlayerStatistics(string name)
        {
            Name = name;
            counters = new Dictionary<PrimaryKey, CounterValueCollection>();
        }

        public CounterValueCollection this[PrimaryKey key]
        {
            get
            {
                if (!counters.ContainsKey(key))
                {
                    counters.Add(key, new CounterValueCollection(counterGroup));
                }
                return counters[key];
            }
        }

        public IEnumerator<KeyValuePair<PrimaryKey, CounterValueCollection>> GetEnumerator()
        {
            return counters.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return counters.GetEnumerator();
        }
    }
}
