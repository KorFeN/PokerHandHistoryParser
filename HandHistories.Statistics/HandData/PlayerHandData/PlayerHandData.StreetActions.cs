using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    partial class PlayerHandData
    {
        public List<HandAction> GetStreetActions(Street street)
        {
            switch (street)
            {
                case Street.Preflop:
                    return PreFlopActions;
                case Street.Flop:
                    return FlopActions;
                case Street.Turn:
                    return TurnActions;
                case Street.River:
                    return RiverActions;
                default:
                    return null;
            }
        }

        List<HandAction> preFlopActions;
        public List<HandAction> PreFlopActions
        {
            get
            {
                if (preFlopActions == null)
                {
                    preFlopActions = handData.PreFlopActions.Where(p => p.PlayerName == playerName).ToList();
                }
                return preFlopActions;
            }
        }

        List<HandAction> flopActions;
        public List<HandAction> FlopActions
        {
            get
            {
                if (flopActions == null)
                {
                    flopActions = handData.FlopActions.Where(p => p.PlayerName == playerName).ToList();
                }
                return flopActions;
            }
        }

        List<HandAction> turnActions;
        public List<HandAction> TurnActions
        {
            get
            {
                if (turnActions == null)
                {
                    turnActions = handData.TurnActions.Where(p => p.PlayerName == playerName).ToList();
                }
                return turnActions;
            }
        }

        List<HandAction> riverActions;
        public List<HandAction> RiverActions
        {
            get
            {
                if (riverActions == null)
                {
                    riverActions = handData.RiverActions.Where(p => p.PlayerName == playerName).ToList();
                }
                return riverActions;
            }
        }
    }
}
