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
        List<Selection> selections = new List<Selection>();

        public MainWindow()
        {
            InitializeComponent();
            InitialiseButtons();
            DisplayTable();

            AnalyseTips();

            LoadTipsters();
            try
            {
                AnalyseMatches();
            }
            catch (Exception e)
            {
                var i = 0;
            }
        }

        private List<string> GetDates(string filePath)
        {
            var dirInfo = new DirectoryInfo(filePath);
            var files = dirInfo.GetFiles("*.txt");
            var list = new List<string>();
            foreach (var file in files)
            {
                var split = file.Name.Split('.');
                list.Add(split[0]);
            }
            return list;
        }

        private void AnalyseTips()
        {
            try
            {
                var now = DateTime.Now;

                var filePath = Environment.CurrentDirectory + "\\Tips";
                var files = GetDates(filePath);

                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Tips");
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Tips\\Holding");
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Tips\\History");

                foreach (var file in files)
                {
                    var dt = new DateHandler().ParseString(file);

                    if (dt < now && dt.AddHours(3) > now)
                    {
                        // move to temp
                        File.Move(Environment.CurrentDirectory + "\\Tips\\" + file + ".txt", Environment.CurrentDirectory + "\\Tips\\Holding\\" + file + ".txt");
                    }
                    else if (dt < now)
                    {
                        // analyse
                        AnalyseFile(file, false);
                        File.Move(Environment.CurrentDirectory + "\\Tips\\" + file + ".txt", Environment.CurrentDirectory + "\\Tips\\History\\" + file + ".txt");
                    }

                    Console.WriteLine(dt.ToString("dd/MM/yyyy"));
                }
                var files2 = GetDates(filePath + "\\Holding");

                foreach (var file in files2)
                {
                    var dt = new DateHandler().ParseString(file);

                    if (dt < now && dt.AddHours(3) > now)
                    {
                        // ignore 
                    }
                    if (dt < now)
                    {
                        // analyse
                        AnalyseFile(file, true);
                        File.Delete(Environment.CurrentDirectory + "\\Tips\\Holding\\" + file + ".txt");
                    }

                    Console.WriteLine(dt.ToString("dd/MM/yyyy"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AnalyseFile(string file, bool holding)
        {
            var won = false;

            var bets = new List<PossibleBet>();

            var fullPath = !holding ? Environment.CurrentDirectory + "\\Tips\\" + file + ".txt" : Environment.CurrentDirectory + "\\Tips\\Holding\\" + file + ".txt";

            var sr = new StreamReader(fullPath);

            var line = sr.ReadLine();
            while (line != null)
            {
                var split = line.Split('@');
                var type = (BetType)Enum.Parse(typeof(BetType), split[0]);
                var pb = new PossibleBet(split[3], split[4], split[1], split[6], type);
                pb.Player = split[5];
                pb.Confidence = float.Parse(split[7]);
                bets.Add(pb);

                var results = new List<Fixture>();
                foreach(var v in LeagueHolder.LeagueList)
                {
                    if(v.Value.Table.Any(t => t.Team == pb.HomeTeam))
                    {
                        results = v.Value.Results;
                        break;
                    }
                }

                Fixture fixture = null;

                foreach (var f in results)
                {
                    if(f.HomeTeam == pb.HomeTeam && f.AwayTeam == pb.AwayTeam)
                    {
                        fixture = f;
                    }
                }

                if(fixture == null) { return; }
                won = IsWinner(pb, fixture);

                var time = file.Split('@');

                var sw = new StreamWriter(Environment.CurrentDirectory + "//Tips//History//" + file + ".txt", true);
                sw.WriteLine(string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}@{7}", pb.Type, pb.When + "@" + time[1], pb.HomeTeam, pb.AwayTeam, pb.Player, pb.Odds, pb.Confidence, won));
                sw.Close();

                line = sr.ReadLine();

            }

            sr.Close();



        }

        private bool IsWinner(PossibleBet pb, Fixture fixture)
        {
            if (pb.Type == BetType.HomeWin)
            {
                if (fixture.HomeScore > fixture.AwayScore) { return true; }
            }
            if (pb.Type == BetType.AwayWin)
            {
                if (fixture.HomeScore < fixture.AwayScore) { return true; }
            }
            if (pb.Type == BetType.Draw)
            {
                if (fixture.HomeScore == fixture.AwayScore) { return true; }
            }
            if (pb.Type == BetType.ToScoreIn90)
            {
                var pNameSplit = pb.Player.Split(' ');
                var pName = pNameSplit[0].Substring(0,1) + " " + pNameSplit[1];
                if (fixture.HomeScorers.Any(g => g.Scorer.Trim() == pName)) { return true; }
                if (fixture.AwayScorers.Any(g => g.Scorer.Trim() == pName)) { return true; }
            }
            if (pb.Type == BetType.Over1AndAHalf)
            {
                if (fixture.HomeScore + fixture.AwayScore > 1) { return true; }
            }
            if (pb.Type == BetType.BTTS)
            {
                if (fixture.HomeScore > 1 && fixture.AwayScore > 1) { return true; }
            }
            return false;
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
            cmdCompetitions.txtTitle.Text = "Competitions";
            cmdStats.txtTitle.Text = "Bet Statistics";
            cmdCurrentBets.txtTitle.Text = "Current Tips";
            cmdBets.txtTitle.Text = "Tip History";
            cmdFixtures.txtTitle.Text = "Matches";
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
            if (leagueIndex < 0) { leagueIndex = LeagueHolder.LeagueList.Count() - 1; }
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
            ls.Owner = this;
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

            foreach (var setup in BotConfigs)
            {
                selections.AddRange(suggestionFetcher.GetBets(setup));
            }

            SaveSetups(selections);
        }

        private void SaveSetups(List<Selection> selections)
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Tips");

            // clear existing
            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory + "\\Tips"))
            {
                File.Delete(file);
            }

            var index = 0; 
            foreach (var bet in selections)
            {
                var date = bet.Date.Replace(':', '£');
                var filePath = Environment.CurrentDirectory + "\\Tips\\" + date;
                Directory.CreateDirectory(filePath);

                var sw = new StreamWriter(filePath + $"\\{index}.txt", true);
                foreach(var element in bet.Items)
                {
                    try
                    {

                        sw.WriteLine(string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}", element.Type, element.When, element.HomeTeam, element.AwayTeam, element.Player, element.Odds, element.Confidence));

                    }
                    catch (Exception e)
                    {
                        var vv = 0;
                    }
                }

                sw.Close();

                index++;

            }


            //foreach (var bet in selections)
            //{
            //    if (bet.Confidence > 45)
            //    {

            //        var date = bet.When.Replace(':', '£');
            //        try
            //        {
            //            var filePath = Environment.CurrentDirectory + "\\Tips\\" + date + ".txt";

            //            var sw = new StreamWriter(filePath, true);

            //            sw.WriteLine(string.Format("{0}@{1}@{2}@{3}@{4}@{5}@{6}", bet.Type, bet.When, bet.HomeTeam, bet.AwayTeam, bet.Player, bet.Odds, bet.Confidence));

            //            sw.Close();
            //        }
            //        catch (Exception e)
            //        {
            //            var vv = 0;
            //        }
            //    }
            //}

            var t = 0;

        }

        private void cmdCurrentBets_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            selections.Clear();
            AnalyseMatches();

            var ss = new SelectionScreen(selections);
            ss.Owner = this;
            ss.Show();
            this.Hide();


            //var page = new CurrentTipsPage(selections);
            //page.Show();
            //page.Owner = this;
            //this.Hide();
        }

        private void cmdFixtures_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var fixturesPage = new FixturePage();
            fixturesPage.Owner = this;
            fixturesPage.Show();
            this.Hide();
        }

        private void cmdBets_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var page = new PreviousTipPage();
            page.Show();
            page.Owner = this;
            this.Hide();
        }
    }
}
