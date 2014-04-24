using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.HandData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    /// <summary>
    /// TODO: This class will contain non-player specific hand information that might be used by multiple conditions
    /// </summary>
    public class GeneralHandData : BasicHandData
    {
        public GeneralHandData(HandHistory handHistory)
            : base (handHistory)
        {
            foreach (var player in handHistory.Players)
            {
                PlayerList.Add(player.PlayerName, new PlayerHandData(handHistory, player.PlayerName));
            }
        }

        public readonly Dictionary<string, PlayerHandData> PlayerList = new Dictionary<string, PlayerHandData>();

        List<HandAction> preFlopActions;
        public List<HandAction> PreFlopActions
        {
            get
            {
                if (preFlopActions == null)
                {
                    preFlopActions = handHistory.HandActions.Street(Objects.Cards.Street.Preflop).ToList();
                }
                return preFlopActions;
            }
        }
    }
}
