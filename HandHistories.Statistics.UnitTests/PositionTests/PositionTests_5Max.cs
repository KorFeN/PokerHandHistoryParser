using HandHistories.Objects.Players;
using HandHistories.Statistics.Positions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics.UnitTests.PositionTests
{
    class PositionTests_5Max : PositionTest
    {
        [Test]
        public void Positions_5Max_Case1()
        {
            var hand = Hand1;
            hand.Players = TestCase1_5Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
            TestMP1(hand);
        }

        [Test]
        public void Positions_5Max_Case2()
        {
            var hand = Hand4;
            hand.Players = TestCase2_5Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
            TestMP1(hand);
        }

        PlayerList TestCase1_5Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_BTN, 1m, 1),
                    new Player(Player_SB, 1m, 2),
                    new Player(Player_BB, 1m, 3),
                    new Player(Player_MP1, 1m, 5),
                    new Player(Player_CO, 1m, 6),
                };
            }
        }

        PlayerList TestCase2_5Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_MP1, 1m, 2),
                    new Player(Player_CO, 1m, 3),
                    new Player(Player_BTN, 1m, 4),
                    new Player(Player_SB, 1m, 5),
                    new Player(Player_BB, 1m, 6),
                };
            }
        }
    }
}
