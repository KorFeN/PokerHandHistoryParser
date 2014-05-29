using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.StreetBaseConditions
{
    public abstract class FoldInstance : IStatisticCondition
    {
        Street _street;

        public FoldInstance(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
            if (action.HandActionType == HandActionType.FOLD)
            {
                ConditionTrigger(generalData, playerHand, action);
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
                        condition = typeof(Flop.FlopBetInstanceCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnBetInstanceCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverBetInstanceCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }

    public abstract class FoldOppertunity : IStatisticCondition
    {
        Street _street;

        public FoldOppertunity(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
            int ActionIndex = action.ActionNumber;
            while (ActionIndex < generalData.handHistory.HandActions.Count)
            {
                HandAction OppertunityAction = generalData.handHistory.HandActions[ActionIndex++];
                if (OppertunityAction.Street != _street)
                {
                    return;
                }

                ConditionTrigger(generalData, generalData.PlayerList[OppertunityAction.PlayerName], OppertunityAction);

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
                        condition = typeof(Flop.FlopBetInstanceCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnBetInstanceCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverBetInstanceCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }
}
