﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.Factory;
using HandHistories.Statistics.Conditions;
using HandHistories.Statistics;
using HandHistories.Statistics.Core;
using HandHistories.Statistics.Statistics;
using System.Diagnostics;
using HandHistories.Statistics.PlayerStats;

namespace HandHistories.Parser.WindowsTestApp
{
    public partial class ParserTestForm : Form
    {
        public ParserTestForm()
        {
            InitializeComponent();

            listBoxSite.Items.Add(SiteName.BossMedia);
            listBoxSite.Items.Add(SiteName.PokerStars);
            listBoxSite.Items.Add(SiteName.PokerStarsFr);
            listBoxSite.Items.Add(SiteName.PokerStarsIt);
            listBoxSite.Items.Add(SiteName.PokerStarsEs);
            listBoxSite.Items.Add(SiteName.FullTilt);
            listBoxSite.Items.Add(SiteName.PartyPoker);
            listBoxSite.Items.Add(SiteName.IPoker);
            listBoxSite.Items.Add(SiteName.OnGame);
            listBoxSite.Items.Add(SiteName.OnGameFr);
            listBoxSite.Items.Add(SiteName.OnGameIt);
            listBoxSite.Items.Add(SiteName.Pacific);
            listBoxSite.Items.Add(SiteName.Entraction);
            listBoxSite.Items.Add(SiteName.Merge);
            listBoxSite.Items.Add(SiteName.WinningPoker);
        }

        private void buttonParse_Click(object sender, EventArgs e)
        {
            if (listBoxSite.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please pick a site");
                return;
            }

            IHandHistoryParserFactory factory = new HandHistoryParserFactoryImpl();
            var handParser = factory.GetFullHandHistoryParser((SiteName) listBoxSite.SelectedItem);

            try
            {
                var hands = handParser.SplitUpMultipleHands(richTextBoxHandText.Text).ToList();
                foreach (var hand in hands)
                {
                    var parsedHand = handParser.ParseFullHandHistory(hand, true);    
                }
                
                MessageBox.Show(this, "Parsed " + hands.Count + " hands.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message + "\r\n" + ex.StackTrace, "Error");
            }
        }

        private void button_Statistics_Click(object sender, EventArgs e)
        {
            if (listBoxSite.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please pick a site");
                return;
            }

            Stopwatch duration = new Stopwatch();
            duration.Start();

            StatisticsEvaluator statEval = new StatisticsEvaluator();

            IStatistic VPIP = BasicHandStatistic.VPIP;
            IStatistic PFR = BasicHandStatistic.PFR;
            IStatistic _3Bet = BasicHandStatistic.ThreeBet;

            statEval.AddStatistic(BasicCounterStatistic.Hands);
            statEval.AddStatistic(VPIP);
            statEval.AddStatistic(PFR);
            statEval.AddStatistic(_3Bet);
            statEval.Initialize();

            IHandHistoryParserFactory factory = new HandHistoryParserFactoryImpl();
            var handParser = factory.GetFullHandHistoryParser((SiteName)listBoxSite.SelectedItem);

            try
            {
                var hands = handParser.SplitUpMultipleHands(richTextBoxHandText.Text).ToList();
                foreach (var hand in hands)
                {
                    var parsedHand = handParser.ParseFullHandHistory(hand, true);
                    GeneralHandData generalHand = new GeneralHandData(parsedHand);
                    statEval.EvaluateHand(generalHand);
                }

                var counters = statEval.GetPlayerCounters(textBox_PlayerName.Text, new KeyFilter()
                {
                    Site = SiteName.PokerStars,
                    PokerFormat = PokerFormat.CashGame,
                    GameType = GameType.NoLimitHoldem,
                    Limits = LimitRange.All,
                    NumPlayersActiveMin = 2,
                    NumPlayersActiveMax = 10
                });

                decimal handsPlayed = Math.Round(BasicCounterStatistic.Hands.GetValue(counters));
                decimal vpipProc = Math.Round(VPIP.GetValue(counters) * 100, 1);//Math.Round(VPIP.Value * 100, 1);
                decimal pfrProc = Math.Round(PFR.GetValue(counters) * 100, 1);
                //decimal threeBetProc = Math.Round(_3Bet.Value * 100, 1);
                string StatisticString = string.Format("{0}/{1}/{2}", vpipProc, pfrProc, "");
                duration.Stop();

                MessageBox.Show(this, string.Format("Player {0} played {1} of {2} hands.\r\n{3}\r\nin {4}ms", new object[]{textBox_PlayerName.Text, handsPlayed, hands.Count, StatisticString, duration.ElapsedMilliseconds}));
            }
            catch (Exception ex)
            {
                duration.Stop();
                MessageBox.Show(this, ex.Message + "\r\n" + ex.StackTrace, "Error"); 
            }
        }
    }
}
