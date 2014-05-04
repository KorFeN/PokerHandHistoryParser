using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Positions
{
    public partial struct PFPosition
    {
        #region LookUp
        static PreFlopPosition[][] PositionLookup = InitPostionLookup();

        private static PreFlopPosition[][] InitPostionLookup()
        {
            PreFlopPosition[][] lookup = new PreFlopPosition[10][];
            lookup[2] = Init2Max();
            lookup[3] = Init3Max();
            lookup[4] = Init4Max();
            lookup[5] = Init5Max();
            lookup[6] = Init6Max();
            return lookup;
        }

        private static PreFlopPosition[] Init2Max()
        {
            return new PreFlopPosition[] { PreFlopPosition.BigBlind };
        }

        private static PreFlopPosition[] Init3Max()
        {
            return new PreFlopPosition[] { PreFlopPosition.SmallBlind, PreFlopPosition.BigBlind };
        }

        private static PreFlopPosition[] Init4Max()
        {
            return new PreFlopPosition[] { PreFlopPosition.SmallBlind, PreFlopPosition.BigBlind, PreFlopPosition.CutOff };
        }

        private static PreFlopPosition[] Init5Max()
        {
            return new PreFlopPosition[] { PreFlopPosition.SmallBlind, PreFlopPosition.BigBlind, PreFlopPosition.MiddlePosition, PreFlopPosition.CutOff };
        }

        private static PreFlopPosition[] Init6Max()
        {
            return new PreFlopPosition[] { PreFlopPosition.SmallBlind, PreFlopPosition.BigBlind, PreFlopPosition.UnderTheGun, PreFlopPosition.MiddlePosition, PreFlopPosition.CutOff };
        }
        #endregion

    }
}
