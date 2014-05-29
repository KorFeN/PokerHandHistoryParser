using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.PreFlop
{
    public class PreflopRaiseCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
        {
            HandAction PFRAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsRaise);
            if (PFRAction != null)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalHand, hand, PFRAction);
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
