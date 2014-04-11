using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Objects.Players;
using HandHistories.Parser.Parsers.Exceptions;
using HandHistories.Parser.Parsers.FastParser.Base;
using HandHistories.Parser.Utils.FastParsing;

namespace HandHistories.Parser.Parsers.FastParser.IPoker
{
    public class IPokerFastParserImpl : HandHistoryParserFastImpl
    {
        const int playerStartIndex = 7;//"<player".Length
        const int actionStartIndex = 7;//"<action".Length

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly bool _isIpoker2;

        public IPokerFastParserImpl(bool isIpoker2 = false)
        {
            _isIpoker2 = isIpoker2;
        }

        public override SiteName SiteName
        {
            get { return (_isIpoker2) ? SiteName.IPoker2 : SiteName.IPoker; }
        }

        public override bool  RequresAdjustedRaiseSizes
        {
	        get 
	        { 
		         return true;
	        }
        }

        public override bool RequiresActionSorting
        {
            get { return true; }
        }

        public override bool RequiresAllInDetection
        {
            get { return true; }
        }

        protected override string[] SplitHandsLines(string handText)
        {
            string[] lines = handText.Split(new char[]{'<'}, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>(lines.Length);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string CurrentLine = lines[i].Trim();
                if (CurrentLine[0] != '/')
                {
                    if (CurrentLine[0] == '?')
                    {
                        continue;
                    }

                    if (lines[i + 1][0] == '/')
                    {
                        if (CurrentLine[CurrentLine.Length - 1] == '>' &&
                            CurrentLine[CurrentLine.Length - 2] == '/')
                        {
                            newLines.Add('<' + CurrentLine);
                        }
                        else
                        {
                            newLines.Add('<' + CurrentLine + '<' + lines[i + 1].Trim());
                            i++;
                        }
                    }
                    else
                    {
                        newLines.Add('<' + CurrentLine);
                    }
                }
                else
                {
                    newLines.Add('<' + CurrentLine);
                }
            }
            newLines.Add('<' + lines[lines.Length - 1]);
            return newLines.ToArray();

            XDocument handDocument = XDocument.Parse(handText);
            return base.SplitHandsLines(handDocument.ToString());
        }

        //protected XDocument GetXDocumentFromLines(string[] handLines)
        //{
        //    string handString = string.Join("", handLines);

        //    XmlReaderSettings xrs = new XmlReaderSettings();
        //    xrs.ConformanceLevel = ConformanceLevel.Fragment;

        //    XDocument doc = new XDocument(new XElement("root"));
        //    XElement root = doc.Descendants().First();

        //    using (StringReader fs = new StringReader(handString))
        //    using (XmlReader xr = XmlReader.Create(fs, xrs))
        //    {
        //        while (xr.Read())
        //        {
        //            if (xr.NodeType == XmlNodeType.Element)
        //            {
        //                root.Add(XElement.Load(xr.ReadSubtree()));
        //            }
        //        }
        //    }

        //    return doc;
        //}

        /*
            <player seat="3" name="RodriDiaz3" chips="$2.25" dealer="0" win="$0" bet="$0.08" rebuy="0" addon="0" />
            <player seat="8" name="Kristi48ru" chips="$6.43" dealer="1" win="$0.23" bet="$0.16" rebuy="0" addon="0" />
        */

        protected int GetSeatNumberFromPlayerLine(string playerLine)
        {
            string seatNumberString = GetPlayerLineAttributeValue(playerLine, "seat");
            return FastInt.Parse(seatNumberString);            
        }

        protected bool IsPlayerLineDealer(string playerLine)
        {
            int dealerValue = FastInt.Parse(GetPlayerLineAttributeValue(playerLine, "dealer"));
            return dealerValue == 1;
        }

        protected decimal GetStackFromPlayerLine(string playerLine)
        {
            string stackString = GetPlayerLineAttributeValue(playerLine, "chips");
            if (stackString[0] == '€' || stackString[0] == '£' || stackString[0] == '$')
	        {
		        stackString = stackString.Substring(1);
            }
            return decimal.Parse(stackString, System.Globalization.CultureInfo.InvariantCulture);
        }

        protected decimal GetWinningsFromPlayerLine(string playerLine)
        {
            string stackString = GetPlayerLineAttributeValue(playerLine, "win")
                .Replace("€", "").Replace("$", "").Replace("£", "");
            return decimal.Parse(stackString, System.Globalization.CultureInfo.InvariantCulture);
        }

        protected string GetNameFromPlayerLine(string playerLine)
        {
            string name = GetPlayerLineAttributeValue(playerLine, "name");
            return name;
        }

        protected string GetPlayerLineAttributeValue(string actionLine, string attribute)
        {
            char firstChar = attribute[0];
            for (int i = actionStartIndex; i < actionLine.Length; i++)
            {
                if (actionLine[i] == ' ')
                {
                    char attributeChar = actionLine[i + 1];
                    if (firstChar == attributeChar)
                    {
                        int startIndex = i + 1 + attribute.Length + 2;//+1 is the offset from ' ' and +2 is the length of "=\"".
                        int endIndex = actionLine.IndexOf('\"', startIndex + 1);
                        return actionLine.Substring(startIndex, endIndex - startIndex);
                    }
                    else
                    {
                        switch (attributeChar)
                        {
                            case 'b'://bet=""
                            case 'w'://win=""
                                i += 6;
                                break;
                            case 'n'://name=""
                            case 's'://seat=""
                                i += 7;
                                break;
                            case 'a'://addon=""
                            case 'r'://rebuy="" and reg_code="" 
                                     //we choose the shortest because the loop will find the space eventually
                            case 'c'://chips=""
                                i += 8;
                                break;
                            case 'd'://dealer=""
                                i += 9;
                                break;
                        }
                    }
                }
            }
            throw new ArgumentException("Atrribute not found: " + attribute);
        }

        protected override int ParseDealerPosition(string[] handLines)
        {

            string[] playerLines = GetPlayerLinesFromHandLines(handLines);

            for (int i = 0; i < playerLines.Count(); i++)
            {
                string playerLine = playerLines[i];
                if (IsPlayerLineDealer(playerLine))
                {
                    return GetSeatNumberFromPlayerLine(playerLine);                    
                }
            }

            throw new Exception("Could not locate dealer");
        }

        protected override DateTime ParseDateUtc(string[] handLines)
        {
            //<startdate>2012-05-28 16:52:05</startdate>
            string dateLine = GetStartDateFromHandLines(handLines);
            int startPos = dateLine.IndexOf('>') + 1;
            int endPos = dateLine.LastIndexOf('<') - 1;
            string dateString = dateLine.Substring(startPos, endPos - startPos + 1);
            DateTime dateTime = DateTime.Parse(dateString);
            //TimeZoneUtil.ConvertDateTimeToUtc(dateTime, TimeZoneType.GMT);

            return dateTime;
        }

        private static readonly Regex SessionGroupRegex = new Regex("<session.*?session>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
        private static readonly Regex GameGroupRegex = new Regex("<game.*?game>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);
        public override IEnumerable<string> SplitUpMultipleHands(string rawHandHistories)
        {
            //Remove XML headers if necessary 
            rawHandHistories = rawHandHistories.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n", "");

            //Two Cases - Case 1, we have a single <session> tag holding multiple <game> tags
            //            Case 2, we have multiple <session> tags each holding a single <game> tag.
            //            We need our enumerable to have only case 2 representations for parsing
            //
            //Performance improvements: we search for a second <game> tag and then search for 
            //an other <session> in reverse
            int sessionIndex = rawHandHistories.IndexOf("<session");
            int game1Index = rawHandHistories.IndexOf("<game");
            int game2Index = rawHandHistories.IndexOf("<game", game1Index + 1);

            if (rawHandHistories.LastIndexOf("<session", game2Index) == sessionIndex)
            {
                //We are case 1 - convert to case 2

                int endOfGeneralInfoIndex = rawHandHistories.IndexOf("</general>");

                if (endOfGeneralInfoIndex == -1)
                {
                    // log the first 1000 chars of the file, so we can at least guess what's the problem
                    logger.Fatal("IPokerFastParserImpl.SplitUpMultipleHands(): Encountered a weird file\r\n{0}", rawHandHistories.Substring(0,1000));
                }

                string generalInfoString = rawHandHistories.Substring(0, endOfGeneralInfoIndex + 10);

                int startIndex = rawHandHistories.IndexOf("<game ", endOfGeneralInfoIndex + 9);
                while (startIndex != -1)
                {
                    int endIndex = FastHandSplitEndIndex(rawHandHistories, startIndex);
                    if (endIndex == -1)
                    {
                        throw new ArgumentException("Did not find end of <game>");
                    }
                    string handText = rawHandHistories.Substring(startIndex, endIndex - startIndex);
                    yield return generalInfoString + "\r\n" + handText + "\r\n</session>";
                    startIndex = rawHandHistories.IndexOf("<game ", endIndex);
                }

                //MatchCollection gameMatches = GameGroupRegex.Matches(rawHandHistories, endOfGeneralInfoIndex + 9);
                //foreach (Match gameMatch in gameMatches)
                //{
                //    string fullGameString = generalInfoString + "\r\n" + gameMatch.Value + "\r\n</session>";
                //    yield return fullGameString;
                //}
            }
            else
            {
                //We are case 2
                MatchCollection matches = SessionGroupRegex.Matches(rawHandHistories);

                foreach (Match match in matches)
                {
                    yield return match.Value;
                }                
            }
        }

        private int FastHandSplitEndIndex(string rawHandHistories, int startIndex)
        {
            for (int i = startIndex; i < rawHandHistories.Length; i++)
            {
                if (rawHandHistories[i] == '<')
                {
                    char currentChar = rawHandHistories[i + 1];
                    switch (currentChar)
                    {
                        case 'p'://"<players>" and "<player "
                            if (rawHandHistories[i + 7] == ' ')
                            {
                                i += 89; //"<player seat="" name="" chips="" dealer="" win="" bet="" rebuy="" addon="" reg_code="" />".Length
                            }
                            else
                            {
                                i += 9;//"<players>".Length
                            }
                            break;
                        case 'a':
                            i += 58;//"<action no="" player="" type="" sum="" cards="" />".Length
                            break;
                        case 'r':
                            i += 13;//"<round no="">".Length
                            break;
                        case '/':
                            if (rawHandHistories.Substring(i + 2, 5) == "game>")
                            {
                                return i + 7;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return rawHandHistories.IndexOf("/game>", startIndex) + 5;
        }

        // For now (4/17/2012) only need Game # in Miner and using Regexes. Will convert to faster mechanism soon.
        private static readonly Regex HandIdRegex = new Regex("(?<=gamecode=\")\\d+", RegexOptions.Compiled);
        protected override long ParseHandId(string[] handLines)
        {
            foreach (var handLine in handLines)
            {
                if (handLine[1] == 'g' && handLine.StartsWith("<game gamecode=\""))
                {
                    const int StartIndex = 16;//"<game gamecode=\".Length
                    int endIndex = handLine.IndexOf('\"', StartIndex);
                    string idString = handLine.Substring(StartIndex, endIndex - StartIndex);
                    return long.Parse(idString);
                }
            }

            throw new HandIdException(handLines[1], "Couldn't find handid");
        }

        protected override string ParseTableName(string[] handLines)
        {
            //<tablename>Ambia, 98575671</tablename>
            string tableNameLine = GetTableNameLineFromHandLines(handLines);
            int tableNameStartIndex = tableNameLine.IndexOf('>') + 1;
            int tableNameEndIndex = tableNameLine.LastIndexOf('<') - 1;

            string tableName = tableNameLine.Substring(tableNameStartIndex, tableNameEndIndex - tableNameStartIndex + 1);

            return tableName;
        }

        protected override SeatType ParseSeatType(string[] handLines)
        {
            string[] playerLines = GetPlayerLinesFromHandLines(handLines);
            int numPlayers = playerLines.Count();

            if (numPlayers <= 2)
            {
                return SeatType.FromMaxPlayers(2);
            }
            if (numPlayers <= 6)
            {
                return SeatType.FromMaxPlayers(6);
            }
            if (numPlayers <= 9)
            {
                return SeatType.FromMaxPlayers(9);
            }

            return SeatType.FromMaxPlayers(10);   
        }

        protected string GetGameTypeLineFromHandLines(string[] handLines)
        {
            //This is the 4th line if we have the <session> tag header
            return handLines[3];
        }

        protected string GetTableNameLineFromHandLines(string[] handLines)
        {
            //This is the 5th line if we have the <session> tag header
            return handLines[4];
        }

        protected string GetStartDateFromHandLines(string[] handLines)
        {
            // in order to find the exact date of the hand we search the startdate of the hand ( and not the table )
            
            for(int i=0; i<= handLines.Count(); i++)
            {
                if(handLines[i][1] == 'g' && handLines[i].Contains("gamecode=\""))
                {
                    return handLines[i + 2];
                }
            }

            // if we're unable to find the dateline for the hand, we just use the date for the table
            return handLines[8];
        }

        protected string[] GetPlayerLinesFromHandLines(string[] handLines)
        {
            /*
              Returns all of the detail lines between the <players> tags
              <players> <-- Line offset 22
                <player seat="1" name="Leichenghui" chips="£1,866.23" dealer="1" win="£0" bet="£5" rebuy="0" addon="0" />
                ...
                <player seat="10" name="CraigyColesBRL" chips="£2,297.25" dealer="0" win="£15" bet="£10" rebuy="0" addon="0" />
              </players>             
             */

            int offset = 23;
            List<string> playerLines = new List<string>();

            string line = handLines[offset];
            //line = line.TrimStart();
            while (offset < handLines.Count() && line[1] != '/')
            {
                playerLines.Add(line);
                offset++;

                if (offset >= handLines.Count())
                    break;

                line = handLines[offset];
                //line = line.TrimStart();
            }

            return playerLines.ToArray();
        }

        protected string[] GetCardLinesFromHandLines(string[] handLines)
        {
            List<string> cardLines = new List<string>();
            for (int i = 0; i < handLines.Length; i++)
            {
                string handLine = handLines[i];
               // handLine = handLine.TrimStart();

                //If we don't have these letters at these positions, we're not a hand line
                if (handLine[1] != 'c' || handLine[7] != 't')
                {
                    continue;
                }

                cardLines.Add(handLine);
            }

            return cardLines.ToArray();
        }

        protected override GameType ParseGameType(string[] handLines)
        {
            /*
             * NLH <gametype>Holdem NL $2/$4</gametype>  
             * NLH <gametype>Holdem PL $2/$4</gametype>    
             * FL  <gametype>Holdem L $2/$4</gametype>
             * PLO <gametype>Omaha PL $0.50/$1</gametype>
             */

            string gameTypeLine = GetGameTypeLineFromHandLines(handLines);

            //If this is an H we're a Holdem, if O, Omaha
            char gameTypeChar = gameTypeLine[10]; 

            if (gameTypeChar == 'O')
            {
                return GameType.PotLimitOmaha;
            }

            //If this is an N, we're NLH, if L - FL
            char holdemTypeChar = gameTypeLine[17]; 

            if (holdemTypeChar == 'L')
            {
                return GameType.FixedLimitHoldem;
            }

            if (holdemTypeChar == 'N')
            {
                return GameType.NoLimitHoldem;
            }

            if (holdemTypeChar == 'P')
            {
                return GameType.PotLimitHoldem;
            }

            throw new Exception("Could not parse GameType for hand.");
        }

        protected override TableType ParseTableType(string[] handLines)
        {
            string tableName = ParseTableName(handLines);

            if (tableName.StartsWith("(Shallow)"))
            {
                return TableType.FromTableTypeDescriptions(TableTypeDescription.Shallow);
            }

            return TableType.FromTableTypeDescriptions(TableTypeDescription.Regular);
        }

        protected override Limit ParseLimit(string[] handLines)
        {
            //GameType line = <gametype>Holdem NL $0.02/$0.04</gametype>
            string gameTypeLine = GetGameTypeLineFromHandLines(handLines);
            int limitStringBeginIndex = gameTypeLine.LastIndexOf(' ') + 1;
            int limitStringEndIndex = gameTypeLine.LastIndexOf('<') - 1;
            string limitString = gameTypeLine.Substring(limitStringBeginIndex,
                                                        limitStringEndIndex - limitStringBeginIndex + 1);

            char currencySymbol = limitString[0];
            Currency currency;

            switch (currencySymbol)
            {
                case '$':
                    currency = Currency.USD;
                    break;
                case '€':
                    currency = Currency.EURO;
                    break;
                case '£':
                    currency = Currency.GBP;
                    break;
                default:
                    throw new LimitException(handLines[0], "Unrecognized currency symbol " + currencySymbol);
            }

            int slashIndex = limitString.IndexOf('/');

            string smallString = limitString.Substring(1, slashIndex - 1);
            decimal small = decimal.Parse(smallString, System.Globalization.CultureInfo.InvariantCulture);

 
            string bigString = limitString.Substring(slashIndex + 2, limitString.Length - (slashIndex + 2));
            decimal big = decimal.Parse(bigString, System.Globalization.CultureInfo.InvariantCulture);

            return Limit.FromSmallBlindBigBlind(small, big, currency);
        }

        public override bool IsValidHand(string[] handLines)
        {
            //Check 1 - Are we in a Session Tag
            if (handLines[0].StartsWith("<session") == false ||
                handLines[handLines.Length - 1].StartsWith("</session") == false)
            {
                return false;
            }

            //Check 2 - Do we have a Game Tag
            if (handLines[19].StartsWith("<game") == false && handLines[20].StartsWith("<game") == false)
            {
                return false;
            }

            //Check 3 - Do we have between 2 and 10 players?
            string[] playerLines = GetPlayerLinesFromHandLines(handLines);
            if (playerLines.Count() < 2 || playerLines.Count() > 10)
            {
                return false;
            }

            //todo add more checks related to action parsing
            return true;
        }

        public override bool IsValidOrCancelledHand(string[] handLines, out bool isCancelled)
        {
            isCancelled = false;
            return IsValidHand(handLines);
        }

        protected override List<HandAction> ParseHandActions(string[] handLines, GameType gameType = GameType.Unknown)
        {
            List<HandAction> actions = new List<HandAction>();

            string[] playerLines = GetPlayerLinesFromHandLines(handLines);
            //The 2nd line after the </player> line is the beginning of the <round> rows


            int offset =  23;

            int startRow = offset + playerLines.Length + 2;

            Street currentStreet = Street.Null;

            for (int i = startRow; i < handLines.Length - 2; i++)
            {
                string handLine = handLines[i];//.TrimStart();

                //If we are starting a new round, update the current street 
                if (handLine[1] == 'r')
                {
                    int roundNumber = GetRoundNumberFromLine(handLine);
                    switch (roundNumber)
                    {
                        case 0:
                        case 1:
                            currentStreet = Street.Preflop;
                            break;
                        case 2:
                            currentStreet = Street.Flop;
                            break;
                        case 3:
                            currentStreet = Street.Turn;
                            break;
                        case 4:
                            currentStreet = Street.River;
                            break;
                        default:
                            throw new Exception("Encountered unknown round number " + roundNumber);
                    }
                }
                //If we're an action, parse the action and act accordingly
                else if (handLine[1] == 'a')
                {
                    HandAction action = GetHandActionFromActionLine(handLine, currentStreet);                   
                    actions.Add(action);
                }
            }

            //Generate the show card + winnings actions
            actions.AddRange(GetWinningAndShowCardActions(handLines));

            return actions;
        }

        private List<HandAction> GetWinningAndShowCardActions(string[] handLines)
        {
            int actionNumber = Int32.MaxValue - 100;

            PlayerList playerList = ParsePlayers(handLines, false, true);

            List<HandAction> winningAndShowCardActions = new List<HandAction>();

            foreach (Player player in playerList)
            {
                if (player.hasHoleCards)
                {
                    HandAction showCardsAction = new HandAction(player.PlayerName, HandActionType.SHOW, 0, Street.Showdown, actionNumber++);    
                    winningAndShowCardActions.Add(showCardsAction);
                }                
            }

            string[] playerLines = GetPlayerLinesFromHandLines(handLines);
            for (int i = 0; i < playerLines.Length; i++)
            {
                string playerLine = playerLines[i];
                decimal winnings = GetWinningsFromPlayerLine(playerLine);                
                if (winnings > 0)
                {
                    string playerName = GetNameFromPlayerLine(playerLine);
                    WinningsAction winningsAction = new WinningsAction(playerName, HandActionType.WINS, winnings, 0, actionNumber++);
                    winningAndShowCardActions.Add(winningsAction);
                }
            }

            return winningAndShowCardActions;
        }

        private HandAction GetHandActionFromActionLine(string handLine, Street street)
        {
            int actionTypeNumber = GetActionTypeFromActionLine(handLine);
            string actionPlayerName = GetPlayerFromActionLine(handLine);
            decimal value = GetValueFromActionLine(handLine);
            int actionNumber = GetActionNumberFromActionLine(handLine);
            HandActionType actionType;
            switch (actionTypeNumber)
            {
                case 0:                
                    actionType = HandActionType.FOLD;
                    break;
                case 1:                 
                    actionType = HandActionType.SMALL_BLIND;
                    break;
                case 2:                 
                    actionType = HandActionType.BIG_BLIND;
                    break;
                case 3:                 
                    actionType = HandActionType.CALL;
                    break;
                case 4:                 
                    actionType = HandActionType.CHECK;
                    break;
                case 5:                 
                    actionType = HandActionType.BET;
                    break;
                case 7:
                    return new AllInAction(actionPlayerName, value, street, false, actionNumber);
                case 8: //Case 8 is when a player sits out at the beginning of a hand 
                case 9: //Case 9 is when a blind isn't posted - can be treated as sitting out
                    actionType = HandActionType.SITTING_OUT;
                    break;
                case 15:
                    actionType = HandActionType.ANTE;
                    break;
                case 23:                 
                    actionType = HandActionType.RAISE;
                    break;
                default:
                    throw new Exception(string.Format("Encountered unknown Action Type: {0} w/ line \r\n{1}", actionTypeNumber, handLine));
            }
            return new HandAction(actionPlayerName, actionType, value, street, actionNumber);
        }

        protected int GetRoundNumberFromLine(string handLine)
        {
            int startPos = handLine.IndexOf(" n") + 5;
            int endPos = handLine.IndexOf('"', startPos) - 1;
            string numString = handLine.Substring(startPos, endPos - startPos + 1);
            return Int32.Parse(numString);            
        }

        protected int GetActionNumberFromActionLine(string actionLine)
        {
            string actionNumString = GetActionAttributeValue(actionLine, "no");
            return Int32.Parse(actionNumString);
        }

        protected string GetPlayerFromActionLine(string actionLine)
        {
            string name = GetActionAttributeValue(actionLine, "player");
            return name;
        }

        protected decimal GetValueFromActionLine(string actionLine)
        {
            string value = GetActionAttributeValue(actionLine, "sum");
            if (value[0] == '$' || value[0] == '€' || value[0] == '£')
            {
                value = value.Substring(1);
            }
            return decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }

        protected int GetActionTypeFromActionLine(string actionLine)
        {
            string actionNumString = GetActionAttributeValue(actionLine, "type");
            return FastInt.Parse(actionNumString);
        }

        protected string GetActionAttributeValue(string actionLine, string attribute)
        {
            char firstChar = attribute[0];
            for (int i = actionStartIndex; i < actionLine.Length; i++)
            {
                if (actionLine[i] == ' ')
                {
                    char attributeChar = actionLine[i + 1];
                    if (firstChar == attributeChar)
                    {
                        int startIndex = i + 1 + attribute.Length + 2;//+1 is the offset from ' ' and +2 is the length of "=\"".
                        int endIndex = actionLine.IndexOf('\"', startIndex + 1);
                        return actionLine.Substring(startIndex, endIndex - startIndex);
                    }
                    else
                    {
                        switch (attributeChar)
                        {
                            case 'n'://no=""
                                i += 5;
                                break;
                            case 'p'://player=""
                                i += 9;
                                break;
                            case 't'://type=""
                                i += 7;
                                break;
                            case 's'://sum=""
                                i += 6;
                                break;
                            case 'c'://cards=""
                                i += 8;
                                break;
                        }
                    }
                }
            }
            throw new ArgumentException("Atrribute not found: " + attribute);
        }

        protected override PlayerList ParsePlayers(string[] handLines, bool ScanSitOutStatus, bool ScanHoleCards)
        {
            /*
                <player seat="3" name="RodriDiaz3" chips="$2.25" dealer="0" win="$0" bet="$0.08" rebuy="0" addon="0" />
                <player seat="8" name="Kristi48ru" chips="$6.43" dealer="1" win="$0.23" bet="$0.16" rebuy="0" addon="0" />
             */

            string[] playerLines = GetPlayerLinesFromHandLines(handLines);

            PlayerList playerList = new PlayerList();

            for (int i = 0; i < playerLines.Length; i++)
            {
                string playerName = GetNameFromPlayerLine(playerLines[i]);
                decimal stack = GetStackFromPlayerLine(playerLines[i]);
                int seat = GetSeatNumberFromPlayerLine(playerLines[i]);
                playerList.Add(new Player(playerName, stack, seat)
                                   {
                                       IsSittingOut = true
                                   });
            }

            if (ScanSitOutStatus)
            {
                List<HandAction> actions = new List<HandAction>();
                const int offset = 23;
                int startRow = offset + playerLines.Length + 2;

                for (int i = startRow; i < handLines.Length - 2; i++)
                {
                    string handLine = handLines[i];//.TrimStart();

                    if (handLine[1] == 'a')
                    {
                        HandAction action = GetHandActionFromActionLine(handLine, Street.Null);
                        actions.Add(action);
                    }
                    else if (handLine[1] == 'r')
                    {
                        bool preFlop = true;
                        int roundNumber = GetRoundNumberFromLine(handLine);
                        switch (roundNumber)
                        {
                            case 2:
                            case 3:
                            case 4:
                                preFlop = false;
                                break;
                        }
                        if (!preFlop)
                        {
                            break;
                        }
                    }
                }

                foreach (var player in playerList)
                {
                    HandAction firstAction = actions.FirstOrDefault(p => p.PlayerName == player.PlayerName);
                    if (firstAction != null && firstAction.HandActionType != HandActionType.SITTING_OUT)
                    {
                        player.IsSittingOut = false;
                    }
                }
            }

            /* 
             * Grab known hole cards for players and add them to the player
             * <cards type="Pocket" player="pepealas5">CA CK</cards>
             */
            if (ScanHoleCards)
            {
                string[] cardLines = GetCardLinesFromHandLines(handLines);

                for (int i = 0; i < cardLines.Length; i++)
                {
                    string handLine = cardLines[i];
                    //handLine = handLine.TrimStart();

                    //To make sure we know the exact character location of each card, turn 10s into Ts (these are recognized by our parser)
                    //Had to change this to specific cases so we didn't accidentally change player names
                    handLine = handLine.Replace("10 ", "T ");
                    handLine = handLine.Replace("10<", "T<");

                    //We only care about Pocket Cards
                    if (handLine[13] != 'P')
                    {
                        continue;
                    }

                    //When players fold, we see a line: <cards type="Pocket" player="pepealas5">X X</cards>, we should skip these lines
                    if (handLine[handLine.Length - 9] == 'X')
                    {
                        continue;
                    }

                    int playerNameStartIndex = 29;
                    int playerNameEndIndex = handLine.IndexOf('"', playerNameStartIndex) - 1;
                    string playerName = handLine.Substring(playerNameStartIndex,
                                                           playerNameEndIndex - playerNameStartIndex + 1);
                    Player player = playerList.First(p => p.PlayerName.Equals(playerName));


                    int playerCardsStartIndex = playerNameEndIndex + 3;
                    int playerCardsEndIndex = handLine.Length - 9;
                    string playerCardString = handLine.Substring(playerCardsStartIndex,
                                                            playerCardsEndIndex - playerCardsStartIndex + 1);
                    string[] cards = playerCardString.Split(' ');
                    if (cards.Length > 0)
                    {
                        player.HoleCards = HoleCards.NoHolecards(player.PlayerName);
                        foreach (string card in cards)
                        {
                            //Suit and rank are reversed in these strings, so we flip them around before adding
                            player.HoleCards.AddCard(new Card(card[1], card[0]));
                        }
                    }
                }
            }
            

            return playerList;
        }

        protected override BoardCards ParseCommunityCards(string[] handLines)
        {
            string boardCards = string.Empty;
            /* 
             * <cards type="Flop" player="">D6 S9 S7</cards>
             * <cards type="Turn" player="">H8</cards>
             * <cards type="River" player="">D5</cards>
             * <cards type="Pocket" player="pepealas5">CA CK</cards>
             */

            string[] cardLines = GetCardLinesFromHandLines(handLines);

            for (int i = 0; i < cardLines.Length; i++)
            {
                string handLine = cardLines[i];
                //handLine = handLine.TrimStart();

                //To make sure we know the exact character location of each card, turn 10s into Ts (these are recognized by our parser)
                handLine = handLine.Replace("10", "T");

                //The suit/ranks are reversed, so we need to reverse them when adding them to our board card string

                //Flop
                if (handLine[13] == 'F')
                {
                    boardCards += new Card(handLine[30], handLine[29]) + "," + new Card(handLine[33], handLine[32]) + "," + new Card(handLine[36], handLine[35]);
                }
                //Turn
                if (handLine[13] == 'T')
                {
                    boardCards += "," + new Card(handLine[30], handLine[29]);
                }
                //River
                if (handLine[13] == 'R')
                {
                    boardCards += "," + new Card(handLine[31], handLine[30]);
                    break;
                }
            }

            return BoardCards.FromCards(boardCards);
        }
    }
}
