using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Statistics.Conditions.StreetConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Conditions.StreetBaseConditions
{
    public abstract class BetOppertunity : IStatisticCondition
    {
        Street _street;
        public BetOppertunity(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction action)
        {
            HandAction Oppertunity = playerHand.GetStreetActions(_street).FirstOrDefault();
            if (Oppertunity == null)
            {
                return;
            }
            if (Oppertunity.HandActionType == HandActionType.BET ||
                Oppertunity.HandActionType == HandActionType.CHECK)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalData, playerHand, Oppertunity);
                }
            }
            else if (Oppertunity.HandActionType == HandActionType.FOLD)
            {
                var PreviousBetAction = generalData.GetStreetActions(_street).FirstOrDefault(p => p.HandActionType == HandActionType.BET);
                if (PreviousBetAction == null || PreviousBetAction.ActionNumber > Oppertunity.ActionNumber)
                {
                    if (ConditionTrigger != null)
                    {
                        ConditionTrigger(generalData, playerHand, Oppertunity);
                    }
                }
            }
        }

        public IEnumerable<Type> PrequisiteConditions
        {
            get {
                Type condition;
                switch (_street)
                {
                    case Street.Flop:
                        condition = typeof(FlopCondition);
                        break;
                    case Street.Turn:
                        condition = typeof(TurnCondition);
                        break;
                    case Street.River:
                        condition = typeof(RiverCondition);
                        break;
                    default:
                        throw new ArgumentException("Street");
                }
                return new Type[] { condition }; }
        }
    }

    public abstract class BetInstance : IStatisticCondition
    {
        Street _street;
        public BetInstance(Street street)
        {
            _street = street;
        }

        public event StatisticConditionTrigger ConditionTrigger;

        public void EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand, HandAction OppertunityAction)
        {
            if (OppertunityAction.HandActionType == HandActionType.BET)
            {
                if (ConditionTrigger != null)
                {
                    ConditionTrigger(generalData, playerHand, OppertunityAction);
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
}
