using HandHistories.Objects.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.StreetConditions
{
    public class TurnCondition : IStatisticCondition
    {
        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
            var FOLD = playerHand.FlopActions.Find(p => p.HandActionType == HandActionType.FOLD);
            if (FOLD == null)
            {
                ConditionTrigger(generalData, playerHand, null);
            }
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(FlopCondition) }; }
        }
    }
}

