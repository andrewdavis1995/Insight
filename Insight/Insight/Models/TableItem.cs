using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Models
{
    public class TableItem
    {
        public string Team { get; set; }
        public int[] TableData = new int[8];

        public TableItem(string team, int[] data)
        {
            Team = team;
            TableData = data;
        }

    }
}
