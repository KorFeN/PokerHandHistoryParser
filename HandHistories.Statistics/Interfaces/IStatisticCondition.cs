using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandHistories.Statistics
{
    public interface IStatisticCondition
    {
        bool EvaluateHand(GeneralHandData generalData, PlayerHandData playerHand);
    }
}
