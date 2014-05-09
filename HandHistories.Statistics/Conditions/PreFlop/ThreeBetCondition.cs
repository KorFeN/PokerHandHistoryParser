using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions
{
    public class ThreeBetInstanceCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            if (action.IsRaise)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalHand, hand, null); 
                }
            }
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(ThreeBetOppertunityCondition) }; }
        }
    }

    public class ThreeBetOppertunityCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction PFRAction)
        {
            //for (int i = PFRAction.ActionNumber + 1; i < generalHand.handHistory.HandActions.Count; i++)
            //{
            //    HandAction action = generalHand.handHistory.HandActions[i];
            //    if (ConditionTrigger != null)
            //    {
            //        ConditionTrigger(generalHand, generalHand.PlayerList[action.PlayerName], action);
            //    }
            //    if (action.IsRaise)
            //    {
            //        break;
            //    }
            //}
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(PlayerHandCondition) }; }
        }
    }
}
