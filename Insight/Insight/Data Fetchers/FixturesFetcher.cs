using Insight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Data_Fetchers
{
    class FixturesFetcher
    {
        public List<Fixture> GetFixtures(string league)
        {
            // get page source
            var source = new WebSourceFetcher().GetPageSource($"http://www.skysports.com/football/competitions/{league}/fixtures");
            var list = new List<Fixture>();

            if (String.IsNullOrEmpty(source)) return list;

            list = ExtractFixtures(source, league);

            return list;
        }

        private List<Fixture> ExtractFixtures(string source, string league)
        {
            List<Fixture> fixtures = new List<Fixture>();

            // Find the position of the first instance of the string "first active" 
            var startPos = source.IndexOf("fixres__item");
            var datePos = source.IndexOf("fixres__header2");
            string cut1 = source;


            // Cut all of the data before the start position off
            //string cut1 = source.Substring(startPos);

            //if (startPos > 100000)
            //{
            //    return fixtures;
            //}

            string date = "";
            var dateCopy = cut1;
            dateCopy = dateCopy.Substring(datePos + 17);
            var endDatePos = dateCopy.IndexOf("</");
            date = dateCopy.Substring(0, endDatePos);


            while (startPos > -1)
            {



                if(datePos < startPos)
                {
                    dateCopy = cut1;
                    dateCopy = dateCopy.Substring(datePos+17);
                    endDatePos = dateCopy.IndexOf("</");
                    date = dateCopy.Substring(0, endDatePos);
                }

                //score1
                var scoreWorking = cut1;

                startPos = scoreWorking.IndexOf("matches__teamscores-side");
                scoreWorking = scoreWorking.Substring(startPos + 26);
                var endPos1 = scoreWorking.IndexOf("</");
                var homeScoreStr = scoreWorking.Substring(0, endPos1).Trim();

                // score2
                startPos = scoreWorking.IndexOf("matches__teamscores-side");
                scoreWorking = scoreWorking.Substring(startPos + 26);
                var endPos2 = scoreWorking.IndexOf("</");
                var awayScoreStr = scoreWorking.Substring(0, endPos2).Trim();


                // Cut the relevant part out of the string
                cut1 = cut1.Substring(startPos);
                    startPos = cut1.IndexOf("fixres__item");

                    // home team
                    cut1 = cut1.Substring(startPos + 12);
                    startPos = cut1.IndexOf("--side1");
                    cut1 = cut1.Substring(startPos + 6);
                    startPos = cut1.IndexOf("target");
                    cut1 = cut1.Substring(startPos + 8);
                    // Find the position of the first instance of the string "\"" (in the substring)
                    int endPos = cut1.IndexOf("</");
                    // Cut the relevant part out of the string
                    string homeTeam = cut1.Substring(0, endPos);
                    

                    // kickoff
                    startPos = cut1.IndexOf("matches__date");
                    cut1 = cut1.Substring(startPos + 17);
                    endPos = cut1.IndexOf("</");
                    var kickoff = cut1.Substring(0, endPos).Trim();
                    
                    // away team
                    startPos = cut1.IndexOf("--side2");
                    cut1 = cut1.Substring(startPos + 6);
                    startPos = cut1.IndexOf("target");
                    cut1 = cut1.Substring(startPos + 8);
                    // Find the position of the first instance of the string "\"" (in the substring)
                    endPos = cut1.IndexOf("</");

                    // Cut the relevant part out of the string
                    string awayTeam = cut1.Substring(0, endPos);
                                        
                    Fixture newFixture = new Fixture(homeTeam, awayTeam, league, date + " @ " + kickoff);
                    fixtures.Add(newFixture);
                //}
                //else
                //{
                //    break;
                //}
                startPos = cut1.IndexOf("fixres__item");
                datePos = cut1.IndexOf("fixres__header2");
            }
            return fixtures;
        }

        int GetDaysInMonth(string monthName)
        {
            switch (monthName)
            {
                case "january": return 31;
                case "february": return DateTime.DaysInMonth(DateTime.Now.Year, 2);
                case "march": return 31;
                case "april": return 30;
                case "may": return 31;
                case "june": return 30;
                case "july": return 31;
                case "august": return 31;
                case "september": return 30;
                case "october": return 31;
                case "november": return 30;
                case "december": return 31;
            }

            return 0;
        }
        int GetMonthID(string monthName)
        {
            switch (monthName)
            {
                case "january": return 1;
                case "february": return 2;
                case "march": return 3;
                case "april": return 4;
                case "may": return 5;
                case "june": return 6;
                case "july": return 7;
                case "august": return 8;
                case "september": return 9;
                case "october": return 10;
                case "november": return 11;
                case "december": return 12;
            }

            return 0;
        }


    }
}
