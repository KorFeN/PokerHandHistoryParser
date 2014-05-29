using HandHistories.Statistics.Conditions.PreFlop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Statistics
{
    public static class PreFlopStatistics
    {
        public static BasicHandStatistic VPIP
        {
            get
            {
                return new BasicHandStatistic(typeof(VPIPOppertunityCondition), typeof(VPIPInstanceCondition), "VPIP");
            }
        }

        public static BasicHandStatistic PFR
        {
            get
            {
                return new BasicHandStatistic(typeof(VPIPOppertunityCondition), typeof(PreflopRaiseCondition), "PFR");
            }
        }

        public static BasicHandStatistic RFI
        {
            get
            {
                return new BasicHandStatistic(typeof(RaiseFirstInOppertunityCondition), typeof(RaiseFirstInInstanceCondition), "RFI");
            }
        }

        public static BasicHandStatistic ThreeBet
        {
            get
            {
                return new BasicHandStatistic(typeof(ThreeBetOppertunityCondition), typeof(ThreeBetInstanceCondition), "3Bet");
            }
        }
    }
}
