using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics
{
    public delegate void StatisticConditionTrigger(GeneralHandData generalHand, PlayerHandData hand, HandAction action);

    public interface IStatisticCondition
    {
        event StatisticConditionTrigger ConditionTrigger;
        void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action);
        IEnumerable<Type> PrequisiteConditions { get; }
    }
}
