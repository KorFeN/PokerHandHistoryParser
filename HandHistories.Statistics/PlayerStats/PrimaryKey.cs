using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Hand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.PlayerStats
{
    public sealed class PrimaryKey
    {
        int _hashCode = -1;

        public PokerFormat PokerFormat { get; private set; }

        public SiteName Site { get; private set; }

        public GameType GameType { get; private set; }

        public Limit Limit { get; private set; }

        public SeatType SeatType { get; private set; }

        public TableType TableType { get; private set; }

        public int NumPlayersActive { get; private set; }

        private PrimaryKey()
        {
        }

        public static PrimaryKey CreateFromHand(HandHistory source)
        {
            return new PrimaryKey()
                {
                    PokerFormat = source.GameDescription.PokerFormat,
                    Site = source.GameDescription.Site,
                    GameType = source.GameDescription.GameType,
                    Limit = source.GameDescription.Limit,
                    SeatType = source.GameDescription.SeatType,
                    TableType = source.GameDescription.TableType,
                    NumPlayersActive = source.NumPlayersActive
                };
        }

        public override bool Equals(object obj)
        {
            PrimaryKey other = obj as PrimaryKey;
            if (other != null)
            {
                return other.GameType == GameType 
                    && other.Limit == Limit 
                    && other.NumPlayersActive == NumPlayersActive 
                    && other.PokerFormat == PokerFormat
                    && other.SeatType.Equals(SeatType)
                    && other.Site == Site
                    && other.TableType.Equals(TableType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (_hashCode == -1)
            {
                _hashCode = GenerateHashCode();
            }
            return _hashCode;
        }

        private int GenerateHashCode()
        {
            return unchecked(
                GameType.GetHashCode() * 
                Limit.GetHashCode() *
                NumPlayersActive *
                PokerFormat.GetHashCode() *
                SeatType.GetHashCode() *
                Site.GetHashCode() *
                TableType.GetHashCode()
                );
           
        }
    }

    public sealed class LimitRange
    {
        public static LimitRange All
        {
            get
            {
                return new LimitRange()
                {
                    LimitMax = Limit.FromLimitEnum(LimitEnum.Limit_500000c_1000000c, Currency.All),
                    LimitMin = Limit.FromLimitEnum(LimitEnum.Limit_1c_2c, Currency.All),
                };
            }
        }

        public Limit LimitMin { get; set; }
        public Limit LimitMax { get; set; }

        public bool InRange(Limit limit)
        {
            return LimitMin.BigBlind <= limit.BigBlind && LimitMax.BigBlind >= limit.BigBlind;
        }
    }

    public sealed class KeyFilter
    {
        public SiteName Site { get; set; }

        public PokerFormat PokerFormat { get; set; }

        public GameType GameType { get; set; }

        public LimitRange Limits { get; set; }

        public int NumPlayersActiveMin { get; set; }

        public int NumPlayersActiveMax { get; set; }

        public bool Check(PrimaryKey key)
        {
            return key.Site == Site
                && key.PokerFormat == PokerFormat
                && Limits.InRange(key.Limit)
                && key.GameType == key.GameType
                && key.NumPlayersActive >= NumPlayersActiveMin
                && key.NumPlayersActive <= NumPlayersActiveMax;
        }
    }
}
