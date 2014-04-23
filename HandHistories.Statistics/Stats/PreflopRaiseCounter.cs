using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Stats
{
    public class PreflopRaiseCounter : IStatisticCounter
    {
        PreflopRaiseCondition condition = new PreflopRaiseCondition();

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

    public class PreflopRaiseCondition : IStatisticCondition
    {
        public bool EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction PFRAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.IsPreFlopRaise);
            if (PFRAction != null)
            {
                return true;
            }
            return false;
        }
    }
}
