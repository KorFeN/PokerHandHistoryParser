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
    class PositionTests_6Max : PositionTest
    {
        [Test]
        public void Positions_6Max_Case1()
        {
            var hand = Hand1;
            hand.Players = TestCase1_6Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
            TestUTG(hand);
            TestMP1(hand);
        }

        [Test]
        public void Positions_6Max_Case2()
        {
            var hand = Hand4;
            hand.Players = TestCase2_6Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
            TestUTG(hand);
            TestMP1(hand);
        }

        PlayerList TestCase1_6Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_BTN, 1m, 1),
                    new Player(Player_SB, 1m, 2),
                    new Player(Player_BB, 1m, 3),
                    new Player(Player_UTG, 1m, 4),
                    new Player(Player_MP1, 1m, 5),
                    new Player(Player_CO, 1m, 6),
                };
            }
        }

        PlayerList TestCase2_6Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_UTG, 1m, 1),
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
