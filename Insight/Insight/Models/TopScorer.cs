using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class TopScorer
    {
        public string Player { get; set; }
        public string Team { get; set; }
        public int Goals { get; set; } = 1;

        public TopScorer(string player, string team)
        {
            Player = player;
            Team = team;
        }
    }
}
