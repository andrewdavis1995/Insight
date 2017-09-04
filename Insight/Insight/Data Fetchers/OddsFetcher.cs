using Insight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Insight.Data_Fetchers
{
    public class OddsFetcher
    {
        public List<string> LoadLinks(string league)
        {
            List<string> list = new List<string>();

            var source = new WebSourceFetcher().GetPageSource($"https://m.skybet.com/football/coupon/10010808");

            var matchIndex = source.IndexOf($"/football/{league}/event/");
            int count = 0;

            while (matchIndex > -1)
            {
                source = source.Substring(matchIndex);
                var linkEnd = source.IndexOf("\"");
                var link = source.Substring(0, linkEnd);

                //var src = new WebSourceFetcher().GetPageSource("http://skybet.com" + link);

                //var titleStart = source.IndexOf("cell-text__line cell-text__line--icon") + 30;
                //source = source.Substring(titleStart);
                //titleStart = source.IndexOf(">") + 1;
                //source = source.Substring(titleStart);
                //titleStart = source.IndexOf("</");
                //var title = source.Substring(0, titleStart);
                //source = source.Substring(titleStart);

                //var split = title.Split(new string[] { " v " }, StringSplitOptions.None);

                //var homeTeam = split[0]; // run through alias calculator
                //var awayTeam = split[1]; // run through alias calculator

                list.Add(link);

                source = source.Substring(20);

                // next one
                matchIndex = source.IndexOf($"/football/{league}/event/");
                count++;
            }

            return list;
        }

        public List<PossibleBet> GetBets(string link)
        {
            var source = new WebSourceFetcher().GetPageSource("http://skybet.com" + link);

            var list = new List<PossibleBet>();

            var index = source.IndexOf("data-event-headline") + 14;
            source = source.Substring(index);
            index = source.IndexOf(">")+1;
            source = source.Substring(index);
            index = source.IndexOf("</");
            var title = source.Substring(0, index);
            source = source.Substring(index);

            index = source.IndexOf("<h2");
            source = source.Substring(index);

            index = source.IndexOf(">");
            source = source.Substring(index);
            index = source.IndexOf("|") + 1;
            source = source.Substring(index);
            index = source.IndexOf("<");
            var date = source.Substring(0, index);

            var split = date.Split('|');
            var fullDate = split[0].Trim() + " @ " + split[1].Trim();

            return list;
        }
    }
}






                //var hOddsStart = source.IndexOf("js-oc-price js-not-in-slip") + 20;
                //source = source.Substring(hOddsStart);
                //hOddsStart = source.IndexOf(">") + 1;
                //source = source.Substring(hOddsStart);
                //hOddsStart = source.IndexOf("</");
                //var hOdds = source.Substring(0, hOddsStart).Trim();
                //source = source.Substring(hOddsStart);

                //var dOddsStart = source.IndexOf("js-oc-price js-not-in-slip") + 20;
                //source = source.Substring(dOddsStart);
                //dOddsStart = source.IndexOf(">") + 1;
                //source = source.Substring(dOddsStart);
                //dOddsStart = source.IndexOf("</");
                //var dOdds = source.Substring(0, dOddsStart).Trim();
                //source = source.Substring(dOddsStart);

                //var aOddsStart = source.IndexOf("js-oc-price js-not-in-slip") + 20;
                //source = source.Substring(aOddsStart);
                //aOddsStart = source.IndexOf(">") + 1;
                //source = source.Substring(aOddsStart);
                //aOddsStart = source.IndexOf("</");
                //var aOdds = source.Substring(0, aOddsStart).Trim();
                //source = source.Substring(aOddsStart);