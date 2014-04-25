using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.Conditions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics.UnitTests.ConditionTests.PreFlop
{
    [TestFixture]
    public class VPIPInstanceTest : ConditionTest
    {
        public VPIPInstanceTest()
            :base(new VPIPInstanceCondition())
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
                    new HandAction(Player1, HandActionType.CALL, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.RAISE, 4, Street.Preflop),
                    new HandAction(Player1, HandActionType.FOLD, 0, Street.Preflop),
                };

                var hand2 = DefaultHandHistory_3Players;
                hand2.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(Player1, HandActionType.RAISE, 3, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.CALL, 2, Street.Preflop),
                };

                return new List<HandHistory>()
                {
                    hand1,
                    hand2
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
                    new HandAction(TestPlayer, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(Player1, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.FOLD, 0, Street.Preflop),
                };

                var hand2 = DefaultHandHistory_3Players;
                hand2.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.CALL, 2, Street.Preflop),
                    new HandAction(Player1, HandActionType.RAISE, 3, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(Player2, HandActionType.CALL, 0, Street.Preflop),
                };

                return new List<HandHistory>()
                {
                    hand1,
                    hand2
                };
            }
        }
    }
}
