using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Statistics.Extensions;

namespace HandHistories.Statistics.Conditions.StreetBaseConditions
{
    public abstract class ContinuationBetOppertunity : IStatisticCondition
    {
        Street _street;
        public ContinuationBetOppertunity(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction betOppertunityAction)
        {
            HandAction Aggressor = generalData.GetPreviousStreetActions(_street).Aggressor();
            if (Aggressor != null && Aggressor.PlayerName == betOppertunityAction.PlayerName)
            {
                ConditionTrigger(generalData, playerHand, betOppertunityAction);
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
                        condition = typeof(Flop.FlopBetOppertunityCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnBetOppertunityCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverBetOppertunityCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }

    public abstract class ContinuationBetInstance : IStatisticCondition
    {
        Street _street;
        public ContinuationBetInstance(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction betOppertunityAction)
        {
            if (betOppertunityAction.HandActionType == HandActionType.BET)
            {
                ConditionTrigger(generalData, playerHand, betOppertunityAction);
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
                        condition = typeof(Flop.FlopContinuationBetOppertunityCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(Turn.TurnContinuationBetOppertunityCondition);
                        break;
                    case Street.River:
                        condition = typeof(River.RiverContinuationBetOppertunityCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition };
            }
        }
    }
}
