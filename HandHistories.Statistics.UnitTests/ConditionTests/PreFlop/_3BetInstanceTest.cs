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
    /// <summary>
    /// The Oppertunity for a preflop raise is the same as the oppertunity for VPIP
    /// </summary>
    [TestFixture]
    public class _3BetInstanceTest : ConditionTest
    {
        public _3BetInstanceTest()
            : base(new ThreeBetInstanceCondition())
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
                    new HandAction(Player2, HandActionType.CALL, 2, Street.Preflop),
                    new HandAction(Player1, HandActionType.RAISE, 4, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.RAISE, 8, Street.Preflop),
                    new HandAction(Player2, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(Player1, HandActionType.CALL, 4, Street.Preflop),
                };

                var hand2 = DefaultHandHistory_3Players;
                hand2.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.RAISE, 4, Street.Preflop),
                    new HandAction(Player1, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.RAISE, 6, Street.Preflop),
                    new HandAction(Player2, HandActionType.RAISE, 16, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.CALL, 8, Street.Preflop),
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
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.RAISE, 4, Street.Preflop),
                    new HandAction(Player1, HandActionType.RAISE, 7, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.RAISE, 14, Street.Preflop),
                    new HandAction(Player2, HandActionType.CALL, 14, Street.Preflop),
                    new HandAction(Player1, HandActionType.CALL, 8, Street.Preflop),
                };

                var hand2 = DefaultHandHistory_3Players;
                hand2.HandActions = new List<HandAction>()
                {
                    new HandAction(Player1, HandActionType.SMALL_BLIND, 1, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.BIG_BLIND, 2, Street.Preflop),
                    new HandAction(Player2, HandActionType.RAISE, 4, Street.Preflop),
                    new HandAction(Player1, HandActionType.FOLD, 0, Street.Preflop),
                    new HandAction(TestPlayer, HandActionType.FOLD, 0, Street.Preflop),
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
