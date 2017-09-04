using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBacksWithUI
{
    public class TeamResult
    {
        public string TeamName { get; }
        public int Score { get; }
        public List<string> Scorers { get; set; }
        public List<string> RedCards { get; set; }

        public TeamResult(string name, int score)
        {
            TeamName = name;
            Score = score;
        }
        public TeamResult(string name, int score, List<string> scorers, List<string> reds)
        {
            TeamName = name;
            Score = score;
            Scorers = scorers;
            RedCards = reds;
        }

    }
}
