using HandHistories.Objects.GameDescription;
using HandHistories.Statistics.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.PlayerStats
{
    public sealed class PlayerStatisticsCollection : IEnumerable<PlayerStatistics>
    {
        StatisticsEvaluator coupledEvaluator;
        readonly CounterGroup coupledCounters;
        Dictionary<string, PlayerStatistics> players = new Dictionary<string, PlayerStatistics>();

        public PlayerStatisticsCollection(StatisticsEvaluator statEval, CounterGroup counters)
        {
            coupledEvaluator = statEval;
            coupledCounters = counters;
        }

        public void AddPlayer(PlayerStatistics player)
        {
            players.Add(player.Name, player);
        }

        public PlayerStatistics this[string PlayerName]
        {
            get
            {
                if (!players.ContainsKey(PlayerName))
                {
                    PlayerStatistics newPlayer = FetchMissingPlayer(PlayerName);
                    newPlayer.CounterGroup = coupledCounters;
                    players.Add(PlayerName, newPlayer);
                }
                return players[PlayerName];
            }
        }

        public delegate PlayerStatistics DelFetchMissingPlayer(string playerName);

        public DelFetchMissingPlayer FetchMissingPlayer = delegate(string playerName)
        {
            return new PlayerStatistics(playerName);
        };

        public IEnumerator<PlayerStatistics> GetEnumerator()
        {
            return players.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return players.Values.GetEnumerator();
        }
    }
}
