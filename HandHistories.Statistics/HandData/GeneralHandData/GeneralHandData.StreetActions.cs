using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    partial class GeneralHandData
    {
        public List<HandAction> GetPreviousStreetActions(Street street)
        {
            switch (street)
            {
                case Street.Flop:
                    return PreFlopActions;
                case Street.Turn:
                    return FlopActions;
                case Street.River:
                    return TurnActions;
                default:
                    return null;
            }
        }

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
                    preFlopActions = handHistory.HandActions.Street(Objects.Cards.Street.Preflop).ToList();
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
                    flopActions = handHistory.HandActions.Street(Street.Flop).ToList();
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
                    turnActions = handHistory.HandActions.Street(Street.Turn).ToList();
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
                    riverActions = handHistory.HandActions.Street(Street.River).ToList();
                }
                return riverActions;
            }
        }
    }
}
