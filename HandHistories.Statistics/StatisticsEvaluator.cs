using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.Core;
using HandHistories.Statistics.PlayerStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{ 
    public class StatisticsEvaluator
    {
        readonly ConditionTree conditionTree = new ConditionTree();
        public readonly CounterGroup counterGroup = new CounterGroup();

        PlayerStatisticsCollection players;

        public PlayerStatisticsCollection Players
        {
            get
            {
                return players;
            }
        }

        void IncrementCounter(PrimaryKey key, PlayerHandData player, int CounterID)
        {
            CounterValueCollection counters;
            if (!player.cachedValueCollections.TryGetValue(counterGroup, out counters))
            {
                counters = players[player.playerName][key];
                player.cachedValueCollections.Add(counterGroup, counters);
            }
            counters.IncrementCounter(CounterID);
        }

        public void AddStatistic(IStatistic stat)
        {
            foreach (var item in stat.Conditions)
            {
                AddCondition(item);
            }
        }

        public void AddCondition(Type condition)
        {
		    conditionTree.AddCondition(condition);
            Counter newCounter = counterGroup.CreateCounter(condition);
            if (newCounter != null)
            {
                newCounter.Count += IncrementCounter;
            }
        }

        public void Initialize()
        {
            conditionTree.InitializeTree();
            counterGroup.Initialize(conditionTree);
            players = new PlayerStatisticsCollection(this, counterGroup);
        }

        public void EvaluateHand(HandHistory hand)
        {
            conditionTree.EvaluateHand(new GeneralHandData(hand));
        }

        public void EvaluateHand(GeneralHandData hand)
        {
            conditionTree.EvaluateHand(hand);
        }

        public CounterValueCollection GetPlayerCounters(string PlayerName, KeyFilter filter)
        {
            var filteredCounters = players[PlayerName].counters.Where(p => filter.Check(p.Key));
            CounterValueCollection result = new CounterValueCollection(counterGroup);
            foreach (var counter in filteredCounters)
            {
                result += counter.Value;
            }
            return result;
        }

        public CounterValueCollection GetPlayerCounters(string PlayerName)
        {
            var filteredCounters = players[PlayerName].counters;
            CounterValueCollection result = new CounterValueCollection(counterGroup);
            foreach (var counter in filteredCounters)
            {
                result += counter.Value;
            }
            return result;
        }
    }
}
