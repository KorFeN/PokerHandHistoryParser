using HandHistories.Objects.Hand;
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
    [TestFixture]
    public class PositionTest
    {
        protected const string Player_SB = "SmallBlind";
        protected const string Player_BB = "BigBlind";
        protected const string Player_UTG = "UnderTheGun";
        protected const string Player_EP1 = "EarlyPosition1";
        protected const string Player_EP2 = "EarlyPosition2";
        protected const string Player_MP1 = "MiddlePosition1";
        protected const string Player_MP2 = "MiddlePosition2";
        protected const string Player_MP3 = "MiddlePosition3";
        protected const string Player_CO = "CutOff";
        protected const string Player_BTN = "Button";

        protected HandHistory Hand1
        {
            get
            {
                return new HandHistory()
                {
                    DealerButtonPosition = 1
                };
            }
        }

        protected HandHistory Hand2
        {
            get
            {
                return new HandHistory()
                {
                    DealerButtonPosition = 2
                };
            }
        }

        protected HandHistory Hand4
        {
            get
            {
                return new HandHistory()
                {
                    DealerButtonPosition = 4
                };
            }
        }

        protected void TestPosition(HandHistory hand, string playerName, PreFlopPosition expectedPosition)
        {
            Assert.AreEqual(expectedPosition, PFPosition.GetPosition(hand, playerName));
        }

        protected void TestBTN(HandHistory hand)
        {
            TestPosition(hand, Player_BTN, PreFlopPosition.Button);
        }
        
        protected void TestCO(HandHistory hand)
        {
            TestPosition(hand, Player_CO, PreFlopPosition.CutOff);
        }

        protected void TestBB(HandHistory hand)
        {
            TestPosition(hand, Player_BB, PreFlopPosition.BigBlind);
        } 

        protected void TestSB(HandHistory hand)
        {
            TestPosition(hand, Player_SB, PreFlopPosition.SmallBlind);
        } 
            
        protected void TestUTG(HandHistory hand)
        {
            TestPosition(hand, Player_UTG, PreFlopPosition.UnderTheGun);
        }

        protected void TestMP1(HandHistory hand)
        {
            TestPosition(hand, Player_MP1, PreFlopPosition.MiddlePosition);
        }  
    }
}
