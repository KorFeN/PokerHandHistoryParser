using HandHistories.Objects.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.StreetConditions
{
    public class FlopCondition : IStatisticCondition
    {
        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get { return null; }
        }
    }
}
