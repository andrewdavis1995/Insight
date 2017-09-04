using Insight.Data_Fetchers;
using Insight.Models;
using Insight.UI_Fetcher;
using Insight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Insight
{
    /// <summary>
    /// Interaction logic for TeamView.xaml
    /// </summary>
    public partial class TeamView : Window
    {
        int CurrentMonth = 8;
        int CurrentYear = 2017;

        StatFetcher statFetcher = new StatFetcher();
        List<Fixture> ResultsToShow = new List<Fixture>();
        
        string SelectedTeam;
        List<Fixture> Results;
        List<Fixture> Fixtures;

        League SelectedLeague;

        public TeamView(string team, League league)
        {
            InitializeComponent();
            SelectedLeague = league;
            SelectedTeam = team;

            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;

            Results = league.Results.Where(r => r.HomeTeam == team || r.AwayTeam == team).ToList();
            Fixtures = league.Fixtures.Where(r => r.HomeTeam == team || r.AwayTeam == team).ToList();

            ResultsToShow.AddRange(Results);

            DisplayStats();

            imgBadge.Source = new BadgeFetcher().GetBadge(SelectedTeam);

            this.KeyDown += IsEscape;
        }

        private void ChangeResultNumber(int number)
        {
            ResultsToShow.Clear();

            if(number == -1)
            {
                ResultsToShow.AddRange(Results);
            }
            else
            {
                ResultsToShow.AddRange(Results.Take(number));
            }

            DisplayStats();
        }

        private void DisplayStats()
        {
            GetYellowCards();
            GetRedCards();
            GetGoals();
            GetRecentReds();

            SetTopScorers();
            SetGoalTimes();

            SetOver1Perc();

            SetPartialTable();

            DisplayMatches();
        }

        void SetPartialTable()
        {
            var results = SelectedLeague.Results.Take(ResultsToShow.Count*SelectedLeague.Table.Count).ToList();

            var context = statFetcher.GetContextTable(SelectedTeam, results, TableType.FullTable);

            var table = context[0] as List<TableItem>;
            var startIndex = (int)context[1] + 1;


            var lth = new TableViewHeader();
            lth.Height = 20;
            LeaguePosStack.Children.Add(lth);

            var badgeFetcher = new BadgeFetcher();

            foreach (var team in table)
            {
                var view = new TableView();
                view.Height = 25;
                view.HorizontalAlignment = HorizontalAlignment.Stretch;
                view.lblTeam.Text = team.Team;
                view.lblPosition.Text = startIndex + ".";
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblGames.Text = team.TableData[0].ToString();
                view.lblGoalDiff.Text = team.TableData[6].ToString();
                view.lblPoints.Text = team.TableData[7].ToString();

                if(team.Team == SelectedTeam)
                {
                    view.Background = new SolidColorBrush(Color.FromArgb(70, 32, 195, 179));
                }

                startIndex++;

                LeaguePosStack.Children.Add(view);
            }
        }

        private void SetOver1Perc()
        {
            var perc = statFetcher.GetOver1Perc(ResultsToShow);

            lblOver1Perc.Text = Math.Round(perc, 1) + "%";
        }

        private void SetGoalTimes()
        {
            var goalsFor = new List<Goal>();
            var goalsAgainst = new List<Goal>();

            foreach(var game in ResultsToShow)
            {
                goalsFor.AddRange(game.GetGoalForObject(SelectedTeam));
                goalsAgainst.AddRange(game.GetGoalAgainstObject(SelectedTeam));
            }


            var goalTimes = new int[19][];
            
            for(int i = 0; i < 19; i++) { goalTimes[i] = new int[2]; }
            
            foreach(var goal in goalsFor) { if (goal.Time.Length < 3) { goalTimes[(int)(int.Parse(goal.Time) / 5)][0]++; } else { goalTimes[(int)(int.Parse(goal.Time.Substring(0, 2)) / 5)][0]++; } }
            foreach(var goal in goalsAgainst) { if (goal.Time.Length < 3) { goalTimes[(int)(int.Parse(goal.Time) / 5)][1]++; } else { goalTimes[(int)(int.Parse(goal.Time.Substring(0, 2)) / 5)][1]++; } }

            goalChart.UpdateValues(goalTimes, goalsFor.Count + goalsAgainst.Count);

        }

        private void GetRecentReds()
        {
            RedCardStack.Children.Clear();

            var result = ResultsToShow.FirstOrDefault();

            List<RedCard> reds = new List<RedCard>();

            if(result.HomeTeam == SelectedTeam) { reds = result.HomeRedCards; }
            if(result.AwayTeam == SelectedTeam) { reds = result.AwayRedCards; }

            if (reds.Any())
            {
                foreach (var red in reds)
                {
                    var view = new StatView();
                    view.lblTeam.Text = red.Player.ToString();
                    view.imgBadge.Source = null;
                    view.lblScore.Text = "";

                    RedCardStack.Children.Add(view);
                }
            }
            else
            {
                NoDataLabel ndl = new NoDataLabel();
                RedCardStack.Children.Add(ndl);
            }
        }
        
        private void SetTopScorers()
        {
            TopScorerStack.Children.Clear();
            
            var allScorers = statFetcher.GetTopScorers(ResultsToShow, SelectedTeam);
            var top5 = allScorers.OrderByDescending(scorer => scorer.Goals).Take(5).ToList();

            if (top5.Any())
            {
                foreach (var player in top5)
                {
                    var view = new StatView();
                    view.lblTeam.Text = player.Player.ToString();
                    Uri goalUri = new Uri("pack://application:,,,/Insight;component/Resources/goal.png", UriKind.RelativeOrAbsolute);

                    view.imgBadge.Source = BitmapFrame.Create(goalUri);
                    view.imgBadge.Margin = new Thickness(8);
                    view.lblScore.Text = player.Goals.ToString();

                    TopScorerStack.Children.Add(view);
                }
            }
            else
            {
                NoDataLabel ndl = new NoDataLabel();
                TopScorerStack.Children.Add(ndl);
            }
        }

        private void IsEscape(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Owner.Show();
                Close();
            }
        }

        private void GetRedCards()
        {
            int count = 0;
            foreach (var game in ResultsToShow)
            {
                count += game.GetTeamRed(SelectedTeam);
            }
            lblTotalReds.Content = count.ToString();
        }

        private void GetGoals()
        {
            int count = 0;
            foreach (var game in ResultsToShow)
            {
                count += game.GetTeamGoals(SelectedTeam);
            }
            lblTotalGoals.Text = count.ToString();
        }

        private void GetYellowCards()
        {
            int count = 0;
            foreach (var game in ResultsToShow)
            {
                count += game.GetTeamYellows(SelectedTeam);
            }
            lblTotalYellows.Content = count.ToString();
        }

        private void DisplayMatches()
        {
            GamesStack.Children.Clear();

            var date = new DateTime(CurrentYear, CurrentMonth, 1);
            var month = date.ToString("MMMM");
            
            SelectedMonth.Content = month;

            var results = Results.Where(result => result.KickOff.Contains(month)).ToList();
            var fixtures = Fixtures.Where(result => result.KickOff.Contains(month)).ToList();

            BadgeFetcher badgeFetcher = new BadgeFetcher();

            if (results.Any() || fixtures.Any())
            {
                foreach (var res in results)
                {
                    TeamFixtureView view = new TeamFixtureView();
                    view.Height = 40;
                    view.txtKickoff.Text = res.KickOff.Split('@')[0];
                    var opponent = res.HomeTeam == SelectedTeam
                        ? res.AwayTeam
                        : res.HomeTeam;
                    view.txtScore.Text = res.HomeScore + " - " + res.AwayScore;

                    view.txtOpponent.Text = opponent;

                    if (res.GetOutcome(SelectedTeam) == Fixture.ResultType.Win)
                    {
                        view.ColorBlock.Background = new SolidColorBrush(Color.FromArgb(255, 45, 181, 43));
                    }
                    else if (res.GetOutcome(SelectedTeam) == Fixture.ResultType.Loss)
                    {
                        view.ColorBlock.Background = new SolidColorBrush(Color.FromArgb(255, 240, 40, 30));
                    }
                    else
                    {
                        view.ColorBlock.Background = new SolidColorBrush(Color.FromArgb(255, 249, 226, 14));
                    }

                    view.imgBadge.Source = badgeFetcher.GetBadge(opponent);

                    view.MouseDown += TeamClicked;

                    GamesStack.Children.Add(view);
                }

                foreach (var res in fixtures)
                {
                    TeamFixtureView view = new TeamFixtureView();
                    view.Height = 40;
                    view.txtKickoff.Text = res.KickOff;
                    var opponent = res.HomeTeam == SelectedTeam
                        ? res.AwayTeam
                        : res.HomeTeam;
                    view.txtScore.Text = " vs. ";

                    view.txtOpponent.Text = opponent;

                    view.MouseDown += TeamClicked;

                    view.imgBadge.Source = badgeFetcher.GetBadge(opponent);

                    GamesStack.Children.Add(view);
                }
            }
            else
            {
                NoDataLabel ndl = new NoDataLabel();
                GamesStack.Children.Add(ndl);
            }

        }
        
        private void TeamClicked(object sender, MouseButtonEventArgs e)
        {
            var teamName = ((TeamFixtureView)sender).txtOpponent.Text;

            var tv = new TeamView(teamName, SelectedLeague);
            tv.Owner = this;
            this.Hide();
            tv.Show();        
        }

        private void CmdAllResults_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(-1);
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CmdLast6Results_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(6);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CmdLast10Games_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(10);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 3);
        }

        private void CmdLast3Results_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(3);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void PreviousMonth(object sender, MouseButtonEventArgs e)
        {
            CurrentMonth--;
            if(CurrentMonth < 1)
            {
                CurrentMonth = 12;
                CurrentYear--;
            }

            DisplayMatches();

        }

        private void NextMonth(object sender, MouseButtonEventArgs e)
        {
            CurrentMonth++;
            if (CurrentMonth > 12)
            {
                CurrentMonth = 1;
                CurrentYear++;
            }

            DisplayMatches();

        }
    }
}
