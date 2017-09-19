using Insight.Data_Fetchers;
using Insight.Holders;
using Insight.Models;
using Insight.UI_Fetcher;
using Insight.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Insight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int leagueIndex = 0;
        public static List<TipsterSetup> BotConfigs = new List<TipsterSetup>();
        List<PossibleBet> selections = new List<PossibleBet>();

        public MainWindow()
        {
            InitializeComponent();
            InitialiseButtons();
            DisplayTable();
            LoadTipsters();
            try
            {
                AnalyseMatches();
            }catch(Exception e)
            {
                var i = 0;
            }
        }

        private void LoadTipsters()
        {
            var dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + "/Configs/");

            foreach (var dir in dirs)
            {
                var split = dir.Split('/');
                var dirName = split[split.Length - 1];

                var setup = new TipsterSetup();
                setup.Load(dirName);
                BotConfigs.Add(setup);
            }
        }

        private void InitialiseButtons()
        {
            cmdNewSetup.txtTitle.Text = "New Setup";
            cmdViewSetups.txtTitle.Text = "View Setups";
            cmdCompetitions.txtTitle.Text = "Competitions";
            cmdStats.txtTitle.Text = "Bet Statistics";
            cmdCurrentBets.txtTitle.Text = "Current Tips";
            cmdBets.txtTitle.Text = "Tip History";
        } 
               
        void DisplayTable()
        {
            lstLeague.Children.Clear();

            var currentLeague = LeagueHolder.LeagueList.ElementAt(leagueIndex);

            var badgeFetcher = new BadgeFetcher();
            imgCompetitionIcon.Source = badgeFetcher.GetLeagueIcon(currentLeague.Value.LeagueName);
            txtCompetitionName.Text = currentLeague.Value.LeagueName;
            
            TableViewHeader lth = new TableViewHeader();
            lth.Height = 20;
            lstLeague.Children.Add(lth);

            var pos = 1;
            foreach (var team in currentLeague.Value.Table)
            {
                var view = new TableView();
                view.HorizontalAlignment = HorizontalAlignment.Stretch;
                view.lblTeam.Text = team.Team;
                view.lblPosition.Text = pos + ".";
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblGames.Text = team.TableData[0].ToString();
                view.lblGoalDiff.Text = team.TableData[6].ToString();
                view.lblPoints.Text = team.TableData[7].ToString();

                pos++;

                lstLeague.Children.Add(view);
            }
        }

        private void MoveLeft(object sender, MouseButtonEventArgs e)
        {
            leagueIndex--;
            if(leagueIndex < 0) { leagueIndex = LeagueHolder.LeagueList.Count() - 1; }
            DisplayTable();
        }

        private void MoveRight(object sender, MouseButtonEventArgs e)
        {
            leagueIndex++;
            if (leagueIndex == LeagueHolder.LeagueList.Count()) { leagueIndex = 0; }
            DisplayTable();
        }
        
        private void ViewCompetitions(object sender, MouseButtonEventArgs e)
        {
            LeagueSelection ls = new LeagueSelection();
            ls.Show();
            this.Hide();
        }

        private void CreateSetup(object sender, MouseButtonEventArgs e)
        {
            var tipsterBotCreation = new TipsterBotCreation();
            tipsterBotCreation.Owner = this;
            tipsterBotCreation.Show();
            this.Hide();
        }


        void AnalyseMatches()
        {
            var suggestionFetcher = new SuggestionFetcher();

            foreach(var setup in BotConfigs)
            {
                selections.AddRange(suggestionFetcher.GetBets(setup));
            }

            SaveSetups(selections);
        }

        private void SaveSetups(List<PossibleBet> selections)
        {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Tips");
                    foreach(var file in Directory.GetFiles(Environment.CurrentDirectory + "\\Tips"))
                    {
                        File.Delete(file);
                    }
            foreach (var bet in selections)
            {
                    var date = bet.When.Replace(':', '£');
                try
                {
                    var filePath = Environment.CurrentDirectory + "\\Tips\\" + date + ".txt";
                                        
                    var sw = new StreamWriter(filePath, true);

                    sw.WriteLine(string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}", bet.Type, bet.When, bet.HomeTeam, bet.AwayTeam, bet.Player, bet.Odds, bet.Confidence));

                    sw.Close();
                }catch(Exception e)
                {
                    var vv = 0;
                }
            }
        }

        private void cmdCurrentBets_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selections.Clear();
            AnalyseMatches();

            var page = new CurrentTipsPage(selections);
            page.Show();
            page.Owner = this;
            this.Hide();
        }
    }
}
