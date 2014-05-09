using HandHistories.Objects.Actions;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Statistics.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics.UnitTests.ConditionTests
{
    public abstract class ConditionTest
    {
        protected const string TestPlayer = "TestPlayer";
        protected const string Player1 = "Player1";
        protected const string Player2 = "Player2";
        protected const string Player3 = "Player3";
        protected const string Player4 = "Player4";
        protected const string Player5 = "Player5";

        protected HandHistory DefaultHandHistory_3Players
        {
            get
            {
                return new HandHistory()
                {
                    Players = new PlayerList(new List<Player>
                        {
                            new Player(TestPlayer, 100, 0),
                            new Player(Player1, 100, 1),
                            new Player(Player2, 100, 2),
                        })
                };
            }
        }

        protected HandHistory DefaultHandHistory_6Players
        {
            get
            {
                return new HandHistory()
                {
                    Players = new PlayerList(new List<Player>
                        {
                            new Player(TestPlayer, 100, 0),
                            new Player(Player1, 100, 0),
                            new Player(Player2, 100, 0),
                            new Player(Player3, 100, 0),
                            new Player(Player4, 100, 0),
                            new Player(Player5, 100, 0),
                        }),
                };
            }
        }

        class ConditionTester : IStatistic
        {
            Type ConditionType;
            public ConditionTester(IStatisticCondition condition)
            {
                ConditionType = condition.GetType();
                Conditions = new List<IStatisticCondition> { condition };
            }

            public IEnumerable<IStatisticCondition> Conditions
            {
                get;
                private set;
            }

            public void Initialize(ConditionTree tree)
            {
                tree.InitializationFinnished -= Initialize;
                tree.GetHandCondition(ConditionType).ConditionTrigger += delegate(GeneralHandData generalHand, PlayerHandData hand, HandAction action)
                {
                    if (hand.playerName == TestPlayer)
                    {
                        Value = 1;
                    } 
                };
            }

            public string Name
            {
                get { return "ConditionTester"; }
            }

            public decimal Value
            {
                get;
                private set;
            }

            public bool Triggered
            {
                get
                {
                    return Value == 1;
                }
            }
        }

        IStatisticCondition condition;

        public ConditionTest(IStatisticCondition conditionToTest)
        {
            condition = conditionToTest;
        }

        public abstract List<HandHistory> TriggerHandHistories{ get; }

        public abstract List<HandHistory> NoTriggerHandHistories { get; }

        void TestForTrigger(bool ExpectedTrigger, HandHistory hand, int ID)
        {
            ConditionTester conditionTest = new ConditionTester(condition);
            ConditionTree testTree = new ConditionTree();
            testTree.AddStatistic(conditionTest);
            testTree.InitializeTree();
            testTree.EvaluateHand(new GeneralHandData(hand));

            Assert.AreEqual(ExpectedTrigger, conditionTest.Triggered, 
                string.Format("Condition {0} Failed. It did{1} trigger on hand with index {2}", 
                condition.GetType().FullName,
                conditionTest.Triggered ? "" : " not", 
                ID)
                );
        }

        [Test]
        public void Condition_Trigger()
        {
            for (int i = 0; i < TriggerHandHistories.Count; i++)
            {
                TestForTrigger(true, TriggerHandHistories[i], i);
            }
        }

        [Test]
        public void Condition_NoTrigger()
        {
            for (int i = 0; i < NoTriggerHandHistories.Count; i++)
            {
                TestForTrigger(false, NoTriggerHandHistories[i], i);
            }
        }
    }
}
