using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.HandData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    public abstract class BasicHandData
    {
        public readonly HandHistory handHistory;

        public BasicHandData(HandHistory pHandHistory)
        {
            handHistory = pHandHistory;
            for (int i = 0; i < pHandHistory.HandActions.Count; i++)
            {
                pHandHistory.HandActions[i].ActionNumber = i;
            }
        }

        HashSet<Type> TriggeredMutexes = new HashSet<Type>();

        /// <summary>
        /// </summary>
        /// <param name="Statistic"></param>
        /// <returns>True if we should continue evaluation</returns>
        public bool CheckAndTriggerMutex(object Statistic)
        {
            Type thisType = Statistic.GetType();
            if (!TriggeredMutexes.Contains(thisType))
            {
                TriggeredMutexes.Add(thisType);
                return true;
            }
            return false;
        }

        CustomHandData customHandData;
        public CustomHandData CustomHandData
        {
            get
            {
                if (customHandData == null)
                {
                    customHandData = new CustomHandData();
                }
                return customHandData;
            }
        }
    }
}
