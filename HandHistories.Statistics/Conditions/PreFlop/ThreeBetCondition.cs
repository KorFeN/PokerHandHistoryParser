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
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction PFRAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsRaise);
            HandAction ThreeBetAction = generalHand.PreFlopActions
                .Where(p => p.IsRaise)
                .Skip(1)
                .FirstOrDefault();
            if (PFRAction != null && PFRAction == ThreeBetAction)
            {
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

    public class ThreeBetOppertunityCondition : IStatisticCondition
    {
        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction PFRAction = generalHand.PreFlopActions
                .FirstOrDefault(p => p.IsRaise);
            HandAction PFReRaiseAction = generalHand.PreFlopActions
                .Where(p => p.IsRaise)
                .Skip(1)
                .FirstOrDefault();
            if (PFRAction != null)
            {
                HandAction ThreeBetOPP = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.ActionNumber > PFRAction.ActionNumber
                && (PFReRaiseAction == null || p.ActionNumber <= PFReRaiseAction.ActionNumber));
                if (ThreeBetOPP != null)
                {
                    hand.CustomHandData.StoreData(GetType(), ThreeBetOPP);
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
            get { return new Type[] { typeof(PlayerHandCondition) }; }
        }
    }
}
