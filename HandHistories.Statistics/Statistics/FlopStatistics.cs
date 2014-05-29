using HandHistories.Statistics.Conditions.Flop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Statistics
{
    public static class FlopStatistics
    {
        public static BasicHandStatistic Bet
        {
            get
            {
                return new BasicHandStatistic(typeof(FlopBetOppertunityCondition), typeof(FlopBetInstanceCondition), "Flop.Bet");
            }
        }

        public static BasicHandStatistic CBet
        {
            get
            {
                return new BasicHandStatistic(typeof(FlopContinuationBetOppertunityCondition), typeof(FlopContinuationBetInstanceCondition), "Flop.CBet");
            }
        }

        public static BasicHandStatistic FoldVsCBet
        {
            get
            {
                return new BasicHandStatistic(typeof(FlopFoldVsCBetOppertunityCondition), typeof(FlopFoldVsCBetInstanceCondition), "Flop.FoldVsCBet");
            }
        }
    }
}
