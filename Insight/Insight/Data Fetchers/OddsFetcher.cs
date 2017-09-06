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

            list.AddRange(GetFullTimeResult(source, title, fullDate));  // outrights
            list.AddRange(GetBTTS(source, title, fullDate));  // BTTS
            list.AddRange(GetOver1(source, title, fullDate));  // 1.5+

            return list;
        }

        public List<PossibleBet> GetFullTimeResult(string source, string fixture, string date)
        {
            List<PossibleBet> bets = new List<PossibleBet>();
            try
            {
                // "Full Time Result"
                var index = source.IndexOf("Full Time Result") + 16;
                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var homeOdds = source.Substring(0, index);

                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var drawOdds = source.Substring(0, index);

                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var awayOdds = source.Substring(0, index);


                var split = fixture.Split(new string[] { " v " }, StringSplitOptions.None);

                bets.Add(new PossibleBet(split[0], split[1], date, homeOdds, BetType.HomeWin));
                bets.Add(new PossibleBet(split[0], split[1], date, drawOdds, BetType.Draw));
                bets.Add(new PossibleBet(split[0], split[1], date, awayOdds, BetType.AwayWin));
            }
            catch (Exception) { }

            return bets;
        }




        public List<PossibleBet> GetBTTS(string source, string fixture, string date)
        {
            List<PossibleBet> bets = new List<PossibleBet>();
            try
            {
                // "BTTS"
                var index = source.IndexOf("Both Teams To Score") + 16;
                source = source.Substring(index);
                index = source.IndexOf("Both Teams To Score") + 16;
                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var yesOdds = source.Substring(0, index);

                var split = fixture.Split(new string[] { " v " }, StringSplitOptions.None);

                bets.Add(new PossibleBet(split[0], split[1], date, yesOdds, BetType.BTTS));
            }
            catch (Exception) { }

            return bets;
        }



        public List<PossibleBet> GetOver1(string source, string fixture, string date)
        {
            List<PossibleBet> bets = new List<PossibleBet>();
            try
            {
                // "BTTS"
                var index = source.IndexOf("Under/Over 1.5 Goals") + 16;
                source = source.Substring(index);
                index = source.IndexOf("Under/Over 1.5 Goals") + 16;
                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var yesOdds = source.Substring(0, index);

                var split = fixture.Split(new string[] { " v " }, StringSplitOptions.None);

                bets.Add(new PossibleBet(split[0], split[1], date, yesOdds, BetType.Over1AndAHalf));
            }
            catch (Exception) { }

            return bets;
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