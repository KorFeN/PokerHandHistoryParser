using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.PreFlop
{
    public class VPIPOppertunityCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            HandAction OppertunityAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsBettingRoundAction);
            if (OppertunityAction != null)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalHand, hand, OppertunityAction);
                }
            }
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(PlayerHandCondition) }; }
        }
    }

    public class VPIPInstanceCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            HandAction VPIPAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.HandActionType == HandActionType.CALL ||
                p.HandActionType == HandActionType.RAISE ||
                p.HandActionType == HandActionType.ALL_IN);
            if (VPIPAction != null)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalHand, hand, VPIPAction);
                }
            }
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[]{ typeof(PlayerHandCondition) }; }
        }
    }
}
