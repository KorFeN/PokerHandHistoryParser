using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    public interface IHandCounter
    {
        void EvaluateHand(GeneralHandData generalData);
    }
}
