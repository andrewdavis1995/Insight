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
    /// Interaction logic for LeagueView.xaml
    /// </summary>
    public partial class LeagueView : Window
    {

        int CurrentMonth = 0;
        int CurrentYear = 2017;

        List<Fixture> ResultsToShow = new List<Fixture>();

        TableType SelectedTableType = TableType.FullTable;

        public League SelectedLeague;
        public StatFetcher statFetcher = new StatFetcher();

        public LeagueView(League league)
        {
            InitializeComponent();
            SelectedLeague = league;

            CurrentMonth = DateTime.Now.Month;

            DisplayMatches();

            ResultsToShow.AddRange(SelectedLeague.Results);

            DisplayStats();

            this.KeyDown += IsEscape;

            ScoreView.txtHomeTeam.MouseDown += TeamClicked;
            ScoreView.txtAwayTeam.MouseDown += TeamClicked;

            ScoreView.imgHomeBadge.MouseDown += TeamImgClicked;
            ScoreView.imgAwayBadge.MouseDown += TeamImgClicked;

        }

        private void TeamClicked(object sender, MouseButtonEventArgs e)
        {
            var teamName = ((TextBlock)sender).Text;

            var tv = new TeamView(teamName, SelectedLeague);
            tv.Owner = this;
            this.Hide();
            tv.Show();
        }

        private void TeamImgClicked(object sender, MouseButtonEventArgs e)
        {
            var teamName = ((Image)sender).Tag.ToString();

            var tv = new TeamView(teamName, SelectedLeague);
            tv.Owner = this;
            this.Hide();
            tv.Show();
        }

        private void IsEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Owner.Show();
                Close();
            }
        }

        void SetTopScoringTeam()
        {
            var list = statFetcher.GetTopScoringTeams(ResultsToShow);
            var top5 = list.Take(5);

            ScoringTeamHolder.Children.Clear();

            var badgeFetcher = new BadgeFetcher();
            foreach (var team in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = team.Team;
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblScore.Text = team.TableData[4].ToString();

                ScoringTeamHolder.Children.Add(view);
            }
        }
        void SetLowestScorers()
        {
            var list = statFetcher.GetLowestScoringTeams(ResultsToShow);
            var top5 = list.Take(5);

            ScoringTeamHolderBottom.Children.Clear();

            var badgeFetcher = new BadgeFetcher();
            foreach (var team in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = team.Team;
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblScore.Text = team.TableData[4].ToString();

                ScoringTeamHolderBottom.Children.Add(view);
            }
        }

        void SetBestDefence()
        {
            var list = statFetcher.GetBestDefence(ResultsToShow);
            var top5 = list.Take(5);

            DefenceHolder.Children.Clear();

            var badgeFetcher = new BadgeFetcher();
            foreach (var team in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = team.Team;
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblScore.Text = team.TableData[5].ToString();

                DefenceHolder.Children.Add(view);
            }
        }
        void SetWorstDefence()
        {
            var list = statFetcher.GetWorstDefence(ResultsToShow);
            var top5 = list.Take(5);

            DefenceBottomHolder.Children.Clear();

            var badgeFetcher = new BadgeFetcher();
            foreach (var team in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = team.Team;
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);
                view.lblScore.Text = team.TableData[5].ToString();

                DefenceBottomHolder.Children.Add(view);
            }
        }

        void SetMostYellows()
        {
            var list = statFetcher.GetMostYellows(ResultsToShow);

            var top5 = list.Take(5);

            TopYellows.Children.Clear();

            var badgeFetcher = new BadgeFetcher();
            foreach (var team in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = team.Field.ToString();
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Field.ToString());
                view.lblScore.Text = team.Value.ToString();

                TopYellows.Children.Add(view);
            }
        }
        void SetRecentReds()
        {
            var list = ResultsToShow.Take(SelectedLeague.NumTeams).ToList();

            var badgeFetcher = new BadgeFetcher();

            RedCardStack.Children.Clear();

            if (list.Any())
            {
                foreach (var fixture in list)
                {
                    foreach (var red in fixture.HomeRedCards)
                    {
                        var view = new StatView();
                        view.lblTeam.Text = red.Player;
                        view.imgBadge.Source = badgeFetcher.GetBadge(red.Team);
                        view.lblScore.Text = "";
                        RedCardStack.Children.Add(view);
                    }
                    foreach (var red in fixture.AwayRedCards)
                    {
                        var view = new StatView();
                        view.lblTeam.Text = red.Player;
                        view.imgBadge.Source = badgeFetcher.GetBadge(red.Team);
                        view.lblScore.Text = "";
                        RedCardStack.Children.Add(view);
                    }
                }

            }
            else
            {
                NoDataLabel ndl = new NoDataLabel();
                RedCardStack.Children.Add(ndl);
            }

        }

        List<TopScorer> SetTopScorers()
        {
            var sorted = statFetcher.GetTopScorers(SelectedLeague.Results);

            var top5 = sorted.Take(5).ToList();

            TopScorerStack.Children.Clear();

            BadgeFetcher badgeFetcher = new BadgeFetcher();
            foreach (var player in top5)
            {
                var view = new StatView();
                view.lblTeam.Text = player.Player;
                view.imgBadge.Source = badgeFetcher.GetBadge(player.Team);
                view.lblScore.Text = player.Goals.ToString();
                TopScorerStack.Children.Add(view);
            }

            return top5;
        }

        void DisplayTable(TableType tableType)
        {
            var table = statFetcher.GetTable(ResultsToShow, tableType);
            SelectedTableType = tableType;

            TableContainer.Children.Clear();

            LargeTableHeader lth = new LargeTableHeader();
            lth.Height = 20;
            TableContainer.Children.Add(lth);

            var badgeFetcher = new BadgeFetcher();

            int pos = 1;
            foreach (var team in table)
            {
                var view = new TableViewLarge();
                view.Height = 28;
                view.lblTeam.Text = team.Team;
                view.imgBadge.Source = badgeFetcher.GetBadge(team.Team);

                view.Tag = TableContainer.Children.Count - 1;
                view.lblPosition.Text = pos + ".";
                view.lblGames.Text = team.TableData[0].ToString();
                view.lblWins.Text = team.TableData[1].ToString();
                view.lblDraws.Text = team.TableData[2].ToString();
                view.lblLosses.Text = team.TableData[3].ToString();
                view.lblGf.Text = team.TableData[4].ToString();
                view.lblGa.Text = team.TableData[5].ToString();
                view.lblGd.Text = team.TableData[6].ToString();
                view.MouseDown += ViewTeam;
                view.lblPoints.Text = team.TableData[7].ToString();

                pos++;

                TableContainer.Children.Add(view);
            }
        }

        void DisplayStats()
        {
            var totalGoals = 0;
            foreach (var game in ResultsToShow)
            {
                totalGoals += game.HomeScore;
                totalGoals += game.AwayScore;
            }

            lblTotalGoals.Text = totalGoals.ToString();

            DisplayTable(TableType.FullTable);

            SetTopScoringTeam();
            SetLowestScorers();
            SetBestDefence();
            SetWorstDefence();

            SetTopScorers();
            SetRecentReds();

            SetMostYellows();

            lblTotalReds.Content = statFetcher.GetTotalRedCount(ResultsToShow);
            lblTotalYellows.Content = statFetcher.GetTotalYellowCount(ResultsToShow);

        }

        private void ChangeResultNumber(int number)
        {
            ResultsToShow.Clear();

            if (number == -1)
            {
                ResultsToShow.AddRange(SelectedLeague.Results);
            }
            else
            {
                ResultsToShow.AddRange(SelectedLeague.Results.Take(number));
            }

            DisplayStats();
        }

        private void ViewTeam(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TableViewLarge;
            var tag = int.Parse(item.Tag.ToString());

            var table = statFetcher.GetTable(ResultsToShow, SelectedTableType);

            var team = table[tag];

            TeamView tv = new TeamView(team.Team, SelectedLeague);
            tv.Show();
            tv.BringIntoView();
            tv.Owner = this;
            this.Hide();
        }

        private void ViewFullTable(object sender, MouseButtonEventArgs e)
        {
            DisplayTable(TableType.FullTable);
            CmdFullTable.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdHomeTable.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdAwayTable.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void ViewHomeTable(object sender, MouseButtonEventArgs e)
        {
            DisplayTable(TableType.HomeTable);
            CmdFullTable.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdHomeTable.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdAwayTable.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void ViewAwayTable(object sender, MouseButtonEventArgs e)
        {
            DisplayTable(TableType.AwayTable);
            CmdFullTable.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdHomeTable.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdAwayTable.BorderThickness = new Thickness(0, 0, 0, 3);
        }

        private void CmdAllResults_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(-1);
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CmdLast3Results_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(3 * SelectedLeague.Table.Count / 2);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CmdLast6Results_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(6 * SelectedLeague.Table.Count / 2);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void CmdLast10Games_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeResultNumber(10 * SelectedLeague.Table.Count / 2);  // change after testing
            CmdAllResults.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast3Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast6Results.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdLast10Games.BorderThickness = new Thickness(0, 0, 0, 3);
        }

        private void DisplayMatches()
        {
            MatchStack.Children.Clear();

            var date = new DateTime(CurrentYear, CurrentMonth, 1);
            var month = date.ToString("MMMM");

            SelectedMonth.Content = month;

            var results = SelectedLeague.Results.Where(result => result.KickOff.Contains(month)).ToList();
            results.Reverse();
            var fixtures = SelectedLeague.Fixtures.Where(result => result.KickOff.Contains(month)).ToList();

            results.AddRange(fixtures);

            BadgeFetcher badgeFetcher = new BadgeFetcher();

            var sorted = results.GroupBy(res => res.KickOff.Split('@')[0]);

            if (sorted.Any())
            {
                foreach (var item in sorted)
                {
                    var lbl = new DateLabel();
                    lbl.LblDate.Text = item.Key;
                    MatchStack.Children.Add(lbl);

                    foreach (var res in item)
                    {
                        FixtureView view = new FixtureView();
                        view.Height = 50;
                        view.Margin = new Thickness(0, 0, 0, 1);
                        view.txtKickoff.Text = res.KickOff.Split('@')[1];

                        view.txtHomeTeam.Text = res.HomeTeam;
                        view.imgHomeBadge.Source = badgeFetcher.GetBadge(res.HomeTeam);
                        view.txtAwayTeam.Text = res.AwayTeam;
                        view.imgAwayBadge.Source = badgeFetcher.GetBadge(res.AwayTeam);

                        if (res.LoadingComplete)
                        {
                            view.txtScore.Text = res.HomeScore + " - " + res.AwayScore;
                        }
                        else
                        {
                            view.txtScore.Text = " vs. ";
                        }


                        int count = MatchStack.Children.OfType<FixtureView>().ToList().Count;

                        view.MouseDown += (sender, args) =>
                        {
                            MatchClicked(res, count.ToString());
                        };
                        

                        MatchStack.Children.Add(view);
                    }
                }
            }
            else
            {
                NoDataLabel ndl = new NoDataLabel();
                MatchStack.Children.Add(ndl);
            }

            MatchScroller.ScrollToTop();
            MatchClicked(results.FirstOrDefault(), "0");
        }

        int currentlyClicked = 0;
        void MatchClicked(Fixture match, string id)
        {
            HomeGoalStack.Children.Clear();
            AwayGoalStack.Children.Clear();
            HomeYellowStack.Children.Clear();
            AwayYellowStack.Children.Clear();

            BadgeFetcher badgeFetcher = new BadgeFetcher();

            int i = int.Parse(id);

            try
            {
                var items = MatchStack.Children.OfType<FixtureView>().ToList();

                ((FixtureView)items[currentlyClicked]).Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                ((FixtureView)items[i]).Background = new SolidColorBrush(Color.FromArgb(70, 32, 195, 179));
            }
            catch (Exception) { }

            currentlyClicked = i;

            ScoreView.Margin = new Thickness(0, 0, 0, 1);
            ScoreView.txtKickoff.Text = match.KickOff.Split('@')[1];

            ScoreView.txtHomeTeam.Text = match.HomeTeam;
            ScoreView.imgHomeBadge.Source = badgeFetcher.GetBadge(match.HomeTeam);
            ScoreView.imgHomeBadge.Tag = match.HomeTeam;

            ScoreView.txtAwayTeam.Text = match.AwayTeam;
            ScoreView.imgAwayBadge.Source = badgeFetcher.GetBadge(match.AwayTeam);
            ScoreView.imgAwayBadge.Tag = match.AwayTeam;

            if (match.LoadingComplete)
            {
                ScoreView.txtScore.Text = match.HomeScore + " - " + match.AwayScore;
            }
            else
            {
                ScoreView.txtScore.Text = " vs. ";
            }

            Uri goalUri = new Uri("pack://application:,,,/Insight;component/Resources/goal.png", UriKind.RelativeOrAbsolute);
            Uri redUri = new Uri("pack://application:,,,/Insight;component/Resources/redCard.png", UriKind.RelativeOrAbsolute);
            Uri yellowUri = new Uri("pack://application:,,,/Insight;component/Resources/yellowCard.png", UriKind.RelativeOrAbsolute);

            foreach (var v in match.HomeScorers)
            {
                StatView view = new StatView();
                view.lblTeam.Text = v.Scorer;
                view.imgBadge.Source = BitmapFrame.Create(goalUri);
                view.imgBadge.Margin = new Thickness(5);
                view.lblScore.Text = v.Time.Length < 3
                    ? v.Time + "'"
                    : v.Time.Substring(0, 2) + "'";

                HomeGoalStack.Children.Add(view);
            }
            foreach (var v in match.AwayScorers)
            {
                StatView view = new StatView();
                view.lblTeam.Text = v.Scorer;
                view.imgBadge.Source = BitmapFrame.Create(goalUri);
                view.imgBadge.Margin = new Thickness(5);
                view.lblScore.Text = v.Time.Length < 3
                    ? v.Time + "'"
                    : v.Time.Substring(0, 2) + "'";

                AwayGoalStack.Children.Add(view);
            }


            foreach (var v in match.HomeRedCards)
            {
                StatView view = new StatView();
                view.lblTeam.Text = v.Player;
                view.imgBadge.Source = BitmapFrame.Create(redUri);
                view.imgBadge.Margin = new Thickness(5);
                view.lblScore.Text = "";

                HomeGoalStack.Children.Add(view);
            }
            foreach (var v in match.AwayRedCards)
            {
                StatView view = new StatView();
                view.lblTeam.Text = v.Player;
                view.imgBadge.Source = BitmapFrame.Create(redUri);
                view.imgBadge.Margin = new Thickness(5);
                view.lblScore.Text = "";

                AwayGoalStack.Children.Add(view);
            }

            if (match.LoadingComplete)
            {
                StatView viewHomeYellow = new StatView();
                viewHomeYellow.lblTeam.Text = match.GetTeamYellows(match.HomeTeam).ToString();
                viewHomeYellow.imgBadge.Source = BitmapFrame.Create(yellowUri);
                viewHomeYellow.imgBadge.Margin = new Thickness(5);
                viewHomeYellow.lblScore.Text = "";
                HomeYellowStack.Children.Add(viewHomeYellow);

                StatView viewAwayYellow = new StatView();
                viewAwayYellow.lblTeam.Text = match.GetTeamYellows(match.AwayTeam).ToString();
                viewAwayYellow.imgBadge.Source = BitmapFrame.Create(yellowUri);
                viewAwayYellow.imgBadge.Margin = new Thickness(5);
                viewAwayYellow.lblScore.Text = "";
                AwayYellowStack.Children.Add(viewAwayYellow);
            }
        }

        private void OpenStats(object sender, MouseButtonEventArgs e)
        {
            CmdStats.BorderThickness = new Thickness(0, 0, 0, 3);
            CmdMatches.BorderThickness = new Thickness(0, 0, 0, 0);

            StatContainer.Visibility = Visibility.Visible;
            MatchContainer.Visibility = Visibility.Hidden;
        }

        private void OpenMatches(object sender, MouseButtonEventArgs e)
        {
            CmdStats.BorderThickness = new Thickness(0, 0, 0, 0);
            CmdMatches.BorderThickness = new Thickness(0, 0, 0, 3);

            StatContainer.Visibility = Visibility.Hidden;
            MatchContainer.Visibility = Visibility.Visible;
        }

        private void PreviousMonth(object sender, MouseButtonEventArgs e)
        {
            CurrentMonth--;
            if (CurrentMonth < 1)
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
