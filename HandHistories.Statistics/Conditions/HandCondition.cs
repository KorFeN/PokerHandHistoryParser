using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics.Conditions
{
    public class PlayerHandCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            if (!hand.handHistory.Cancelled)
            {
                Player player = hand.handHistory.Players.FirstOrDefault(p => p.PlayerName == hand.playerName);
                if (player != null && !player.IsSittingOut)
                {
                    if (ConditionTrigger != null)
                    {
                        ConditionTrigger(generalHand, hand);
                    }
                }
            }
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return null; }
        }
    }
}
