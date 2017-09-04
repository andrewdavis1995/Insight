using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class Accumulator
    {
        public List<Bet> AccaItems { get; set; }

        public Accumulator(List<Bet> items)
        {
            AccaItems = items;
        }
        public Accumulator() { }

    }
}
