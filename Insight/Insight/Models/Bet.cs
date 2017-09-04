using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class Bet
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string When { get; set; }
        public string Odds { get; set; } = "1/1";
        public int Validated { get; set; } = 0;
        public bool Won { get; set; } = false;

        public string Name{ get; set; }


        public Bet(string homeTeam, string awayTeam, string date, string odds, string name)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
            Odds = odds;
            Name = name;
        }
        public Bet(string homeTeam, string awayTeam, string date, string odds)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
            Odds = odds;
        }
        public Bet(string homeTeam, string awayTeam, string date)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            When = date;
        }
        public Bet(Bet baseBet)
        {
            HomeTeam = baseBet.HomeTeam;
            AwayTeam = baseBet.AwayTeam;
            When = baseBet.When;
            Odds = baseBet.Odds;
        }

        public double GetOddsDouble()
        {
            var split = Odds.Split('/');
            var numerator = double.Parse(split[0]);
            var denominator = double.Parse(split[1]);

            return numerator / denominator;
        }

    }
}
