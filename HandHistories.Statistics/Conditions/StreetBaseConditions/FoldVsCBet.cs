using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Extensions;

namespace HandHistories.Statistics.Conditions.StreetBaseConditions
{
    public class FoldVsCBetInstance : IStatisticCondition
    {
        Street _street;

        public FoldVsCBetInstance(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction oppertunityAction)
        {
            if (oppertunityAction.HandActionType == HandActionType.FOLD)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalData, playerHand, oppertunityAction);
                }
            }
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get
            {
                Type condition;
                switch (_street)
                {
                    case Street.Flop:
                        condition = typeof(Flop.FlopFoldVsCBetOppertunityCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnFoldVsCBetOppertunityCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverFoldVsCBetOppertunityCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }

    public class FoldVsCBetOppertunity : IStatisticCondition
    {
        Street _street;

        public FoldVsCBetOppertunity(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction CBetAction)
        {
            for (int i = CBetAction.ActionNumber + 1; i < generalData.handHistory.HandActions.Count; i++)
            {
                HandAction OppertunityAction = generalData.handHistory.HandActions[i];

                if (OppertunityAction.Street != _street)
                {
                    return;
                }

                if (OppertunityAction.IsBettingRoundAction && ConditionTrigger != null)
                {
                    ConditionTrigger(generalData, generalData.PlayerList[OppertunityAction.PlayerName], OppertunityAction);
                }

                if (OppertunityAction.IsRaise)
                {
                    return;
                }
            }
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get
            {
                Type condition;
                switch (_street)
                {
                    case Street.Flop:
                        condition = typeof(Flop.FlopContinuationBetInstanceCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnContinuationBetInstanceCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverContinuationBetInstanceCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }
}
