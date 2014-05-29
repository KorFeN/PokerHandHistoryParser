using HandHistories.Objects.Actions;
using HandHistories.Objects.Hand;
using HandHistories.Statistics.HandData;
using HandHistories.Statistics.PlayerStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics
{
    /// <summary>
    /// TODO: This class will contain non-player specific hand information that might be used by multiple conditions
    /// </summary>
    public partial class GeneralHandData : BasicHandData
    {
        public GeneralHandData(HandHistory handHistory)
            : base (handHistory)
        {
            foreach (var player in handHistory.Players)
            {
                PlayerList.Add(player.PlayerName, new PlayerHandData(this, player.PlayerName));
            }
        }

        public readonly Dictionary<string, PlayerHandData> PlayerList = new Dictionary<string, PlayerHandData>();

        PrimaryKey primaryKey;
        public PrimaryKey Key
        {
            get
            {
                if (primaryKey == null)
                {
                    primaryKey = PrimaryKey.CreateFromHand(handHistory);
                }
                return primaryKey;
            }
        }
    }
}
