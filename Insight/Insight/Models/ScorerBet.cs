using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class ScorerBet : Bet
    {
        public string Scorer { get; set; }
        public string ScorerTeam { get; set; }

        public ScorerBet(string scorer, string team, Bet b) : base (b)
        {
            Scorer = scorer;
            ScorerTeam = team;
        }

    }
}
