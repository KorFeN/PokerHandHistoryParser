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
    class PositionTests_4Max : PositionTest
    {
        [Test]
        public void Positions_4Max_Case1()
        {
            var hand = Hand1;
            hand.Players = TestCase1_4Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
        }

        [Test]
        public void Positions_4Max_Case2()
        {
            var hand = Hand4;
            hand.Players = TestCase2_4Max;
            TestBTN(hand);
            TestCO(hand);
            TestBB(hand);
            TestSB(hand);
        }

        PlayerList TestCase1_4Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_BTN, 1m, 1),
                    new Player(Player_SB, 1m, 2),
                    new Player(Player_BB, 1m, 4),
                    new Player(Player_CO, 1m, 6),
                };
            }
        }

        PlayerList TestCase2_4Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_CO, 1m, 2),
                    new Player(Player_BTN, 1m, 4),
                    new Player(Player_SB, 1m, 5),
                    new Player(Player_BB, 1m, 8),
                };
            }
        }
    }
}
