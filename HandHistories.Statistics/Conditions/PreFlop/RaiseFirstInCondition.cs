using HandHistories.Objects.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.PreFlop
{
    public class RaiseFirstInInstanceCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction OppertunityAction)
        {
            if (OppertunityAction.IsRaise)
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
            get { return new Type[] { typeof(RaiseFirstInOppertunityCondition) }; }
        }
    }

    public class RaiseFirstInOppertunityCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction EmptyAction)
        {
            for (int i = 0; i < generalHand.PreFlopActions.Count; i++)
            {
                HandAction action = generalHand.PreFlopActions[i];
                if (action.IsBettingRoundAction)
                {
                    if (action.PlayerName == hand.playerName &&
                        ConditionTrigger != null)
                    {
                        ConditionTrigger(generalHand, generalHand.PlayerList[action.PlayerName], action);
                    }
                }

                if (action.IsRaise)
                {
                    break;
                }
            }
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(PlayerHandCondition) }; }
        }
    }
}
