using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
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
    public class PlayerHandData
    {
        public readonly HandHistory handHistory;
        public readonly string playerName;
        List<HandAction> playerActions;

        public PlayerHandData(HandHistory hand, string PlayerName)
        {
            handHistory = hand;
            playerName = PlayerName;
        }

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
    }
}
