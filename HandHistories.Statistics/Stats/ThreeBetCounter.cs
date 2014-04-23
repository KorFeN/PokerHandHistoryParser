using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Stats
{
    public class ThreeBetCounter : IStatisticCounter
    {
        ThreeBetCondition condition = new ThreeBetCondition();

        public int Count
        {
            get;
            private set;
        }

        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            if (condition.EvaluateHand(generalHand, hand))
            {
                Count++;
            }
        }
    }

    public class ThreeBetCondition : IStatisticCondition
    {
        public bool EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction PFRAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsRaise);
            HandAction ThreeBetAction = hand.handHistory.HandActions
                .Street(Street.Preflop)
                .Where(p => p.IsRaise)
                .Skip(1)
                .FirstOrDefault();
            if (PFRAction != null && PFRAction == ThreeBetAction)
            {
                return true;
            }
            return false;
        }
    }
}
