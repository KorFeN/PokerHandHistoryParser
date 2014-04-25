using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions
{
    public class PreflopRaiseCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction PFRAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsRaise);
            if (PFRAction != null)
            {
                hand.CustomHandData.StoreData(this.GetType(), PFRAction);
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalHand, hand);
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
