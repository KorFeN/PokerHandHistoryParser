using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    /// <summary>
    /// TODO: This class will contain non-player specific hand information that might be used by multiple conditions
    /// </summary>
    public class GeneralHandData
    {
        HandHistory hand;

        public GeneralHandData(HandHistory handHistory)
        {
            hand = handHistory;
        }
    }
}
