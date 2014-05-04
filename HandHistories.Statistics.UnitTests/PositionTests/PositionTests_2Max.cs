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
    class PositionTests_2Max : PositionTest
    {
        [Test]
        public void Positions_2Max_Case1()
        {
            var hand = Hand1;
            hand.Players = TestCase1_2Max;
            TestBTN(hand);
            TestBB(hand);
        }

        [Test]
        public void Positions_2Max_Case2()
        {
            var hand = Hand4;
            hand.Players = TestCase2_2Max;
            TestBTN(hand);
            TestBB(hand);
        }

        PlayerList TestCase1_2Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_BTN, 1m, 1),
                    new Player(Player_BB, 1m, 4),
                };
            }
        }

        PlayerList TestCase2_2Max
        {
            get
            {
                return new PlayerList()
                {
                    new Player(Player_BB, 1m, 1),
                    new Player(Player_BTN, 1m, 4),
                };
            }
        }
    }
}
