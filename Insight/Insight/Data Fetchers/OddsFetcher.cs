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
            list.AddRange(GetHalfWithMostGoals(source, title, fullDate));  // HWMG
            list.AddRange(GetScorers(source, title, fullDate));  // TSI90

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

                var aliasFetcher = new AliasFetcher();
                var home = aliasFetcher.SkybetNameToFixtureName(split[0]);
                var away = aliasFetcher.SkybetNameToFixtureName(split[1]);

                bets.Add(new PossibleBet(home, away, date, homeOdds, BetType.HomeWin));
                bets.Add(new PossibleBet(home, away, date, drawOdds, BetType.Draw));
                bets.Add(new PossibleBet(home, away, date, awayOdds, BetType.AwayWin));
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
                var aliasFetcher = new AliasFetcher();
                var home = aliasFetcher.SkybetNameToFixtureName(split[0]);
                var away = aliasFetcher.SkybetNameToFixtureName(split[1]);

                bets.Add(new PossibleBet(home, away, date, yesOdds, BetType.BTTS));
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
                var aliasFetcher = new AliasFetcher();
                var home = aliasFetcher.SkybetNameToFixtureName(split[0]);
                var away = aliasFetcher.SkybetNameToFixtureName(split[1]);

                bets.Add(new PossibleBet(home, away, date, yesOdds, BetType.Over1AndAHalf));
            }
            catch (Exception) { }

            return bets;
        }

        public List<PossibleBet> GetHalfWithMostGoals(string source, string fixture, string date)
        {
            List<PossibleBet> bets = new List<PossibleBet>();
            try
            {
                // "BTTS"
                var index = source.IndexOf("Half with Most Goals") + 16;
                source = source.Substring(index);
                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var firstOdds = source.Substring(0, index);

                index = source.IndexOf("\"odds\">") + 7;
                source = source.Substring(index);
                index = source.IndexOf("</");
                var secondOdds = source.Substring(0, index);

                var split = fixture.Split(new string[] { " v " }, StringSplitOptions.None);

                var aliasFetcher = new AliasFetcher();
                var home = aliasFetcher.SkybetNameToFixtureName(split[0]);
                var away = aliasFetcher.SkybetNameToFixtureName(split[1]);
                bets.Add(new PossibleBet(home, away, date, firstOdds, BetType.FirstHalfMostGoals));
                bets.Add(new PossibleBet(home, away, date, secondOdds, BetType.SecondHalfMostGoals));
            }
            catch (Exception) { }

            return bets;
        }

        public List<PossibleBet> GetScorers(string source, string fixture, string date)
        {
            List<PossibleBet> bets = new List<PossibleBet>();
            try
            {
                var split = fixture.Split(new string[] { " v " }, StringSplitOptions.None);
                
                var index = source.IndexOf("To Score a Brace") + 16;
                source = source.Substring(index);
                index = source.IndexOf("<th scope=\"row\">");
                source = source.Substring(index);
                var endIndex = source.IndexOf("No Goalscorer");

                //index = source.IndexOf("<span>");

                bool home = true;

                while (index > -1 && index < endIndex)
                {
                    index = 16;
                    source = source.Substring(index);

                    var tempIndex = source.IndexOf("<span>");
                    if(tempIndex < 50)
                    {
                        source = source.Substring(tempIndex + 6);
                    }

                    index = source.IndexOf("<");
                    var player = source.Substring(0, index).Trim();
                    source = source.Substring(index);
                    index = source.IndexOf("\"odds\">") + 7;
                    source = source.Substring(index);
                    index = source.IndexOf("\"odds\">") + 7;
                    source = source.Substring(index);
                    index = source.IndexOf("\"odds\">") + 7;
                    source = source.Substring(index);
                    index = source.IndexOf("</");
                    var playerOdds = source.Substring(0, index);


                    var aliasFetcher = new AliasFetcher();
                    var homeT = aliasFetcher.SkybetNameToFixtureName(split[0]);
                    var awayT = aliasFetcher.SkybetNameToFixtureName(split[1]);

                    if (!player.Contains('/'))
                    {
                        bets.Add(new PossibleBet(homeT, awayT, date, playerOdds, player, BetType.ToScoreIn90, home));
                    }
                    else
                    {
                        home = false;
                    }
                    index = source.IndexOf("<th scope=\"row\">");
                    source = source.Substring(index);
                    endIndex = source.IndexOf("No Goalscorer");

                }

                var stop = 0;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

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