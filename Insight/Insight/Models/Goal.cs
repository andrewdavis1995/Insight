using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class Goal
    {
        public string Scorer { get; set; }
        public string Team { get; set; }
        public string Time { get; set; }

        public Goal (string player, string team, string time)
        {
            Scorer = player;
            Team = team;
            // time is string as some of them are "90 + 7", for instance
            Time = time;
        }

    }
}
