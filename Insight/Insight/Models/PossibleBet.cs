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

        public BetType Type { get; set; }


        public PossibleBet(string homeTeam, string awayTeam, string date, string odds, BetType type)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
            Odds = odds;
            Type = type;
        }

        public double GetOddsDouble()
        {
            var split = Odds.Split('/');
            var numerator = double.Parse(split[0]);
            var denominator = double.Parse(split[1]);

            return numerator / denominator;
        }

    }

    public enum BetType
    {
        HomeWin, Draw, AwayWin, BTTS, Over1AndAHalf
    }

}
