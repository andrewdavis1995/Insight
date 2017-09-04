using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class RedCard
    {
        public string Player { get; set; }
        public string Team { get; set; }

        public RedCard(string player, string team)
        {
            Player = player;
            Team = team;
        }
    }
}
