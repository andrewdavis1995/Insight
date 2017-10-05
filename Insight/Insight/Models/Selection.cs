using System;
using System.Collections.Generic;

namespace Insight.Models
{
    public class Selection
    {
        public string Name { get; }

        public List<PossibleBet> Items = new List<PossibleBet>();

        public string Date { get; }

        public Selection(string name, string date, List<PossibleBet> items)
        {
            Name = name;
            Items = items;
            Date = date;
        }
        public Selection(string name, string date, PossibleBet item)
        {
            Name = name;
            Items.Add(item);
            Date = date;
        }

        public double GetOverallOdds()
        {
            var start = 1d;

            foreach(var item in Items)
            {
                start *= (1 + item.GetOddsDouble());
            }

            return Math.Round(start, 2);
        }

        public string GetDescription()
        {
            if(Items.Count > 1)
            {
                var type = "";
                if (Items[0].Type == BetType.HomeWin) { type = "Outright"; }
                if (Items[0].Type == BetType.AwayWin) { type = "Outright"; }
                if (Items[0].Type == BetType.BTTS) { type = "Goals"; }
                if (Items[0].Type == BetType.Over1AndAHalf) { type = "Goals"; }

                return Items.Count + "-fold " + type + " acca";
            }


            var description = "";
            foreach(var item in Items)
            {
                description += item.GetOutput() + ", ";
            }
            description = description.Substring(0, description.Length - 2);
            return description;
        }

    }
}