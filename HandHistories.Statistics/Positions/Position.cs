using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.Positions
{
    public partial struct PFPosition
    {

        public static PreFlopPosition GetPosition(HandHistory hand, string PlayerName)
        {
            if (hand.Players.Count > 6)
            {
                throw new Exception("Dont supprt more than 6 players");
            }
            int ButtonSeat = hand.DealerButtonPosition;
            Player currentPlayer = hand.Players[PlayerName];
            int buttonDistance;

            if (currentPlayer.SeatNumber == ButtonSeat)
            {
                return PreFlopPosition.Button;
            }
            else if (currentPlayer.SeatNumber > ButtonSeat)
            {
                buttonDistance = hand.Players.Count(p => p.SeatNumber > ButtonSeat && p.SeatNumber < currentPlayer.SeatNumber);
            }
            else
            {
                buttonDistance = hand.Players.Count(p => p.SeatNumber > ButtonSeat || p.SeatNumber < currentPlayer.SeatNumber);
            }

            return PositionLookup[hand.Players.Count][buttonDistance];
        }
    }

    [Flags]
    public enum PreFlopPosition : byte
    {
        None = 0x0,
        EarlyPosition = 0x1 << 0,
        MiddlePosition = 0x1 << 1,
        CutOff = 0x1 << 2,
        Button = 0x1 << 3,
        SmallBlind = 0x1 << 4,
        BigBlind = 0x1 << 5,
        UnderTheGun = 0x1 << 6,
    }
}
