using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class PossibleBet
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string When { get; set; }
        public string Odds { get; set; } = "1/1";
        public int Validated { get; set; } = 0;
        public bool Won { get; set; } = false;
        public string Player { get; set; }
        public bool HomePlayer { get; set; }

        public float Confidence { get; set; }

        public BetType Type { get; set; }


        public PossibleBet(string homeTeam, string awayTeam, string date, string odds, BetType type)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
            Odds = odds;
            Type = type;
        }
        public PossibleBet(string homeTeam, string awayTeam, string date, string odds, string player, BetType type, bool home)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
            Odds = odds;
            Type = type;
            Player = player;
            HomePlayer = home;
        }

        public double GetOddsDouble()
        {
            var split = Odds.Split('/');
            var numerator = double.Parse(split[0]);
            var denominator = double.Parse(split[1]);

            return numerator / denominator;
        }

        public string GetOutput()
        {
            switch (Type)
            {
                case BetType.HomeWin:
                    return HomeTeam + " to win";
                case BetType.AwayWin:
                    return AwayTeam + " to win";
                case BetType.BTTS:
                    return "Both Teams To Score";
                case BetType.Over1AndAHalf:
                    return "Over 1.5 Goals";
                case BetType.ToScoreIn90:
                    return Player + " to score";
            }

            return "";

        }

    }

    public enum BetType
    {
        HomeWin, Draw, AwayWin, BTTS, Over1AndAHalf, FirstHalfMostGoals, SecondHalfMostGoals, ToScoreIn90
    }

}
