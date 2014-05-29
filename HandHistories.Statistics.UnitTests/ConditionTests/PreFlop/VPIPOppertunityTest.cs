using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.Conditions.PreFlop;
using NUnit.Framework;
using System.Collections.Generic;

namespace HandHistories.Statistics.UnitTests.ConditionTests.PreFlop
{
    [TestFixture]
    public class VPIPOppertunityTest : ConditionTest
    {
        public VPIPOppertunityTest()
            : base(typeof(VPIPOppertunityCondition))
        {
        }

        public override List<HandHistory> TriggerHandHistories
        {
            get
            {
                var hand1 = DefaultHandHistory_3Players;
                hand1.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(Player1, HandActionType.RAISE, 3, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.FOLD, 0, Street.Preflop),
                };

                return new List<HandHistory>()
                {
                    hand1
                };
            }
        }

        public override List<HandHistory> NoTriggerHandHistories
        {
            get
            {
                var hand1 = DefaultHandHistory_3Players;
                hand1.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(Player1, HandActionType.FOLD, 0, Street.Preflop),
                };

                return new List<HandHistory>()
                {
                    hand1
                };
            }
        }
    }
}
