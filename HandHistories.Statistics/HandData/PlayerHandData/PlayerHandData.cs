using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Statistics.Core;
using HandHistories.Statistics.HandData;
using HandHistories.Statistics.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics
{
    /// <summary>
    /// TODO: This class will contain player specific hand information that might be used by multiple conditions
    /// </summary>
    public partial class PlayerHandData : BasicHandData
    {
        public readonly string playerName;
        public readonly GeneralHandData handData;
        readonly Player Player;

        public PlayerHandData(GeneralHandData hand, string PlayerName)
            : base(hand.handHistory)
        {
            handData = hand;
            playerName = PlayerName;
            Player = hand.handHistory.Players[PlayerName];
        }

        internal Dictionary<CounterGroup, CounterValueCollection> cachedValueCollections = new Dictionary<CounterGroup, CounterValueCollection>();

        List<HandAction> playerActions;
        public List<HandAction> PlayerActions
        {
            get
            {
                if (playerActions == null)
                {
                    playerActions = handHistory.HandActions.Where(p => p.PlayerName == playerName).ToList();
                }
                return playerActions;
            }
        } 

        public PreFlopPosition Position
        {
            get
            {
                return PFPosition.GetPosition(handData.handHistory, playerName);
            }
        }

        public override string ToString()
        {
            return playerName;
        }
    }
}
