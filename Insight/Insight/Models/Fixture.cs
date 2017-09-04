using Insight.Data_Fetchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class Fixture
    {
        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string League { get; }
        public string KickOff { get; }
        public string Link { get; }

        public int HomeScore { get; }
        public int AwayScore { get; }


        public List<Goal> HomeScorers { get; set; } = new List<Goal>();
        public List<Goal> AwayScorers { get; set; } = new List<Goal>();

        public List<RedCard> HomeRedCards { get; set; } = new List<RedCard>();
        public List<RedCard> AwayRedCards { get; set; } = new List<RedCard>();

        public int homeBookings = 0;
        public int awayBookings = 0;

        public bool LoadingComplete { get; set; } = false;


        public Fixture(string homeTeam, string awayTeam, string league, string kickoff)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
            KickOff = kickoff;
        }

        public Fixture(string homeTeam, string awayTeam, string league, string kickoff, string link, int hScore, int aScore)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
            KickOff = kickoff;
            Link = link;
            HomeScore = hScore;
            AwayScore = aScore;
        }



        public int GetTeamYellows(string team)
        {
            if (HomeTeam == team) { return homeBookings; }
            if (AwayTeam == team) { return awayBookings; }
            return 0;
        }

        public int GetTeamRed(string team)
        {
            if (HomeTeam == team) { return HomeRedCards.Count; }
            if (AwayTeam == team) { return AwayRedCards.Count; }
            return 0;
        }

        public List<Goal> GetGoalForObject(string team)
        {
            if (HomeTeam == team) { return HomeScorers; }
            if (AwayTeam == team) { return AwayScorers; }
            return new List<Goal>();
        }
        public List<Goal> GetGoalAgainstObject(string team)
        {
            if (HomeTeam == team) { return AwayScorers; }
            if (AwayTeam == team) { return HomeScorers; }
            return new List<Goal>();
        }

        public int GetTeamGoals(string team)
        {
            if (HomeTeam == team) { return HomeScore; }
            if (AwayTeam == team) { return AwayScore; }
            return 0;
        }
        public int GetTeamGoalsAgainst(string team)
        {
            if (AwayTeam == team) { return HomeScore; }
            if (HomeTeam == team) { return AwayScore; }
            return 0;
        }




        public void GetOutcome(string player, ref int points, ref int gf, ref int ga, ref int wins, ref int draws, ref int losses)
        {
            int playerScore = 0, oppositionScore = 0;

            if (HomeTeam == player)
            {
                playerScore = HomeScore;
                oppositionScore = AwayScore;
            }
            else
            {
                playerScore = AwayScore;
                oppositionScore = HomeScore;
            }

            if (playerScore > oppositionScore)
            {
                points += 3;
                wins += 1;
            }
            else if (playerScore == oppositionScore)
            {
                points += 1;
                draws += 1;
            }
            else
            {
                losses += 1;
            }

            gf += playerScore;
            ga += oppositionScore;

        }

        public enum ResultType
        {
            Win, 
            Loss, 
            Draw
        }

        public ResultType GetOutcome(string player)
        {
            int playerScore = 0, oppositionScore = 0;

            if (HomeTeam == player)
            {
                playerScore = HomeScore;
                oppositionScore = AwayScore;
            }
            else
            {
                playerScore = AwayScore;
                oppositionScore = HomeScore;
            }

            if (playerScore > oppositionScore)
            {
                return ResultType.Win;
            }
            else if (playerScore == oppositionScore)
            {
                return ResultType.Draw;
            }
            else
            {
                return ResultType.Loss;
            }            

        }

        public void AnalyseReport()
        {
            Thread thr = new Thread(StartAnalysis);
            thr.Start();
        }

        void StartAnalysis()
        {
            WebSourceFetcher wsf = new WebSourceFetcher();
            

            var split = Link.Split('/');

            var url = "";

            for (int i = 0; i < split.Length - 1; i++)
            {
                url += split[i] + "/";
            }

            url += "stats/" + split[split.Length - 1];

            var source = wsf.GetPageSource(url);


            var copy = source;

            try
            {
                
                var index = source.IndexOf("match-head__scorers");
                source = source.Substring(index + 20);
                var endIndex = source.IndexOf("</div>");
                var firstTeamSrc = source.Substring(0, endIndex);
                var listH = firstTeamSrc.Split(new string[] { "<span>", "</span>", "</p>", "", "\\n" }, StringSplitOptions.None).ToList();
                listH.RemoveAt(0);
                listH.RemoveAll(IsBlank);
                
                             


                for(var i = 0; i < listH.Count; i++)
                {
                    var scorer = listH[i];

                    var bracketIndex = scorer.IndexOf("(");
                    var name = scorer.Substring(0, bracketIndex);

                    // check if goal AND RED, deal with different
                    var fullTime = scorer.Substring(bracketIndex);
                    fullTime = fullTime.Substring(1, fullTime.Length - 2);

                    var timesSplit = fullTime.Split(',').ToList();

                    timesSplit.RemoveAll(IsBlank);

                    foreach (var time in timesSplit)
                    {
                        try
                        {
                            if (time.Contains("red-card"))
                            {
                                HomeRedCards.Add(new RedCard(name, HomeTeam));
                                i++;
                            }
                            else
                            {
                                var fTime = time.Replace(")", "");
                                fTime = fTime.Replace("(", "");
                                fTime = fTime.Replace(",", "");

                                if (!fTime.Contains("og"))
                                {
                                    HomeScorers.Add(new Goal(name, HomeTeam, fTime));
                                }
                                else
                                {
                                    HomeScorers.Add(new Goal("Own Goal - " + name, HomeTeam, fTime));
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                    // read times, red cards etc                    
                }



                


                index = source.IndexOf("match-head__scorers");
                source = source.Substring(index);
                endIndex = source.IndexOf("</div>");
                var secondTeamSrc = source.Substring(0, endIndex);
                var listA = secondTeamSrc.Split(new string[] { "<span>", "</span>", "</p>", "" }, StringSplitOptions.None).ToList();
                listA.RemoveAt(0);
                listA.RemoveAll(IsBlank);


                for (var i = 0; i < listA.Count; i++)
                {
                    var scorer = listA[i];

                    var bracketIndex = scorer.IndexOf("(");
                    var name = scorer.Substring(0, bracketIndex);

                    // check if goal AND RED, deal with different
                    var fullTime = scorer.Substring(bracketIndex);
                    fullTime = fullTime.Substring(1, fullTime.Length - 2);

                    var timesSplit = fullTime.Split(',').ToList();
                    timesSplit.RemoveAll(IsBlank);

                    foreach (var time in timesSplit)
                    {
                        try
                        {
                            if (time.Contains("red-card"))
                            {
                                AwayRedCards.Add(new RedCard(name, AwayTeam));
                                i++;
                            }
                            else
                            {
                                var fTime = time.Replace(")", "");
                                fTime = fTime.Replace("(", "");
                                fTime = fTime.Replace(",", "");

                                if (!fTime.Contains("og"))
                                {
                                    AwayScorers.Add(new Goal(name, AwayTeam, fTime));
                                }
                                else
                                {
                                    AwayScorers.Add(new Goal("Own Goal - " + name, AwayTeam, fTime));
                                }
                            }
                        }
                        catch (Exception) { }
                    }

                }



                index = copy.IndexOf("Yellow Cards");
                copy = copy.Substring(index);
                index = copy.IndexOf("match-stat-home");
                copy = copy.Substring(index);
                index = copy.IndexOf(">") + 1;
                copy = copy.Substring(index);
                var outdex = copy.IndexOf("</");
                var yellowsStrH = copy.Substring(0, outdex);

                homeBookings = int.Parse(yellowsStrH);


                copy = copy.Substring(index);
                index = copy.IndexOf("match-stat-away");
                copy = copy.Substring(index);
                index = copy.IndexOf(">") + 1;
                copy = copy.Substring(index);
                outdex = copy.IndexOf("</");
                var yellowsStrA = copy.Substring(0, outdex);

                awayBookings = int.Parse(yellowsStrA);


            }
            catch (Exception) { }
            
            // get url source, extract
            LoadingComplete = true;
        }
        private static bool IsBlank(string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
    }
}
