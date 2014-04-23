using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics.Stats
{
    public class PlayerHandCounter : IStatisticCounter
    {
        PlayerHandCondition condition = new PlayerHandCondition();

        public int Count
        {
            get;
            private set;
        }

        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            if (condition.EvaluateHand(generalHand, hand))
            {
                Count++;
            }
        }
    }

    public class PlayerHandCondition : IStatisticCondition
    {
        public bool EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            if (!hand.handHistory.Cancelled)
            {
                Player player = hand.handHistory.Players.FirstOrDefault(p => p.PlayerName == hand.playerName);
                if (player != null && !player.IsSittingOut)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
