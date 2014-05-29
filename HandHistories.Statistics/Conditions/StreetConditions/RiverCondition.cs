using HandHistories.Objects.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.StreetConditions
{
    public class RiverCondition : IStatisticCondition
    {
        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
            var FOLD = playerHand.TurnActions.Find(p => p.HandActionType == HandActionType.FOLD);
            if (FOLD == null)
            {
                ConditionTrigger(generalData, playerHand, null);
            }
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return new Type[] { typeof(TurnCondition) }; }
        }
    }
}
