using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.Factory;
using HandHistories.Statistics.Stats;
using HandHistories.Statistics;
using HandHistories.Statistics.Core;
using System.Diagnostics;

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

            var StatTree = new ConditionTree();
            StatTree.AddCondition(typeof(PlayerHandCondition));
            StatTree.AddCondition(typeof(VPIPInstanceCondition));
            StatTree.AddCondition(typeof(PreflopRaiseCondition));
            StatTree.AddCondition(typeof(ThreeBetInstanceCondition));
            StatTree.AddCondition(typeof(ThreeBetOppertunityCondition));
            StatTree.InitializeTree();

            var handsPlayed = new SimpleStatCounter(StatTree.GetHandCondition(typeof(PlayerHandCondition)));
            var vpipCounter = new SimpleStatCounter(StatTree.GetHandCondition(typeof(VPIPInstanceCondition)));
            var pfrCounter = new SimpleStatCounter(StatTree.GetHandCondition(typeof(PreflopRaiseCondition)));
            var _3BetCounterINST = new SimpleStatCounter(StatTree.GetHandCondition(typeof(ThreeBetInstanceCondition)));
            var _3BetCounterOPP = new SimpleStatCounter(StatTree.GetHandCondition(typeof(ThreeBetOppertunityCondition)));

            IHandHistoryParserFactory factory = new HandHistoryParserFactoryImpl();
            var handParser = factory.GetFullHandHistoryParser((SiteName)listBoxSite.SelectedItem);

            try
            {
                var hands = handParser.SplitUpMultipleHands(richTextBoxHandText.Text).ToList();
                foreach (var hand in hands)
                {
                    var parsedHand = handParser.ParseFullHandHistory(hand, true);
                    GeneralHandData generalHand = new GeneralHandData(parsedHand);
                    PlayerHandData compiledHand = generalHand.PlayerList.ContainsKey(textBox_PlayerName.Text)
                        ? generalHand.PlayerList[textBox_PlayerName.Text] 
                        : new PlayerHandData(parsedHand, textBox_PlayerName.Text);
                    StatTree.EvaluateHand(generalHand, compiledHand);
                }

                double vpipProc = Math.Round((double)vpipCounter.Count / (double)handsPlayed.Count * 100, 1);
                double pfrProc = Math.Round((double)pfrCounter.Count / (double)handsPlayed.Count * 100, 1);
                double threeBetProc = Math.Round((double)_3BetCounterINST.Count / (double)_3BetCounterOPP.Count * 100, 1);
                string StatisticString = string.Format("{0}/{1}/{2}", vpipProc, pfrProc, threeBetProc);
                duration.Stop();
                MessageBox.Show(this, string.Format("Player {0} played {1} of {2} hands.\r\n{3}\r\nin {4}ms", new object[]{textBox_PlayerName.Text, handsPlayed.Count, hands.Count, StatisticString, duration.ElapsedMilliseconds}));
            }
            catch (Exception ex)
            {
                duration.Stop();
                MessageBox.Show(this, ex.Message + "\r\n" + ex.StackTrace, "Error");
            }
        }
    }
}
