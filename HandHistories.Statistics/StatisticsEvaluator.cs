using HandHistories.Objects.GameDescription;
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
        ConditionTree conditionTree = new ConditionTree();

        readonly CounterGroup statsGroup = new CounterGroup();

        PlayerStatisticsCollection players;

        void IncrementCounter(string PlayerName, PrimaryKey key, int CounterID)
        {
            players[PlayerName][key].IncrementCounter(CounterID);
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
            Counter newCounter = statsGroup.CreateCounter(condition);
            if (newCounter != null)
            {
                newCounter.Count += IncrementCounter;
            }
        }

        public void Initialize()
        {
            conditionTree.InitializeTree();
            statsGroup.Initialize(conditionTree);
            players = new PlayerStatisticsCollection(this, statsGroup);
        }

        public void EvaluateHand(GeneralHandData hand)
        {
            conditionTree.EvaluateHand(hand);
        }

        public CounterValueCollection GetPlayerCounters(string PlayerName, KeyFilter filter)
        {
            var filteredCounters = players[PlayerName].counters.Where(p => filter.Check(p.Key));
            CounterValueCollection result = new CounterValueCollection(statsGroup);
            foreach (var counter in filteredCounters)
            {
                result += counter.Value;
            }
            return result;
        }
    }
}
