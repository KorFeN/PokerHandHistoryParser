using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Stats
{
    public class VPIPCounter
    {
        VPIPInstanceCondition condition = new VPIPInstanceCondition();

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

    public class VPIPInstanceCondition : IStatisticCondition
    {
        public bool EvaluateHand(GeneralHandData generalHand, PlayerHandData hand)
        {
            HandAction VPIPAction = hand.PlayerActions.Street(Street.Preflop)
                .FirstOrDefault(p => p.HandActionType == HandActionType.CALL ||
                p.HandActionType == HandActionType.RAISE ||
                p.HandActionType == HandActionType.ALL_IN);
            if (VPIPAction != null)
            {
                return true;
            }
            return false;
        }
    }
}
