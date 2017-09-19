using Insight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Data_Fetchers
{
    public class StatFetcher
    {
        public List<TableItem> GetTopScoringTeams(List<Fixture> results)
        {
            var table = GetTable(results, TableType.FullTable);

            var list = table.OrderBy(s => s.TableData[4]).ToList();
            list.Reverse();
            return list;
        }
        public List<TableItem> GetLowestScoringTeams(List<Fixture> results)
        {
            var table = GetTable(results, TableType.FullTable);

            var list = table.OrderBy(s => s.TableData[4]).ToList();
            return list;
        }
        public List<TableItem> GetBestDefence(List<Fixture> results)
        {
            var table = GetTable(results, TableType.FullTable);
            var list = table.OrderBy(s => s.TableData[5]).ToList();
            list.Reverse();
            return list;
        }
        public List<TableItem> GetWorstDefence(List<Fixture> results)
        {
            var table = GetTable(results, TableType.FullTable);
            var list = table.OrderBy(s => s.TableData[5 ]).ToList();
            return list;
        }


        public int GetTotalRedCount(List<Fixture> results)
        {
            int count = 0;

            foreach (var game in results)
            {
                count += game.HomeRedCards.Count;
                count += game.AwayRedCards.Count;
            }

            return count;
        }

        public int GetTotalYellowCount(List<Fixture> results)
        {
            int count = 0;

            foreach (var game in results)
            {
                count += game.homeBookings;
                count += game.awayBookings;
            }

            return count;
        }


        public List<TopScorer> GetTopScorers(List<Fixture> results, string team)
        {
            return GetTopScorers(results).Where(scorer => scorer.Team == team).ToList();
        }
                
        public List<TopScorer> GetTopScorers(List<Fixture> results)
        {
            List<TopScorer> allScorers = new List<TopScorer>();

            foreach (var game in results)
            {
                foreach (var scorer in game.HomeScorers)
                {
                    bool found = false;

                    for (int i = 0; i < allScorers.Count; i++)
                    {
                        if (allScorers[i].Player == scorer.Scorer && allScorers[i].Team == scorer.Team)
                        {
                            allScorers[i].Goals++;
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        allScorers.Add(new TopScorer(scorer.Scorer, scorer.Team));
                    }
                }
                foreach (var scorer in game.AwayScorers)
                {
                    bool found = false;

                    for (int i = 0; i < allScorers.Count; i++)
                    {
                        if (allScorers[i].Player == scorer.Scorer && allScorers[i].Team == scorer.Team)
                        {
                            allScorers[i].Goals++;
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        allScorers.Add(new TopScorer(scorer.Scorer, scorer.Team));
                    }
                }
            }

            var sorted = allScorers.OrderByDescending(s => s.Goals);
            return sorted.ToList();
        }

        public float GetOver1Perc(List<Fixture> results)
        {
            int count = 0;

            foreach(var res in results)
            {
                if(res.HomeScore + res.AwayScore > 1)
                {
                    count++;
                }
            }

            return ((float)count / (float)results.Count) * 100f;

        }

        public List<Stat> GetMostYellows(List<Fixture> results)
        {
            var list = new List<Stat>();

            var table = GetTable(results, TableType.FullTable);

            foreach (var team in table)
            {
                int count = 0;
                foreach (var match in results)
                {
                    count += match.GetTeamYellows(team.Team);
                }

                list.Add(new Stat(team.Team, count));
            }

            list = list.OrderByDescending(s => s.Value).ToList();
            return list;
        }

        public List<TableItem> GetTable(List<Fixture> results, TableType tableType)
        {
            List<TableItem> table = new List<TableItem>();
            
            var teams = GetTeamList(results);

            // loop through players in league - replace for foreach
            foreach (var v in teams)
            {
                int points = 0;
                int gf = 0;
                int ga = 0;
                int wins = 0;
                int draws = 0;
                int losses = 0;

                // change to accomodate home/away tables
                var matches = new List<Fixture>();

                if (tableType == TableType.FullTable) { matches = results.Where(f => f.HomeTeam == v || f.AwayTeam == v).ToList(); };
                if (tableType == TableType.HomeTable) { matches = results.Where(f => f.HomeTeam == v).ToList(); };
                if (tableType == TableType.AwayTable) { matches = results.Where(f => f.AwayTeam == v).ToList(); };
                
                foreach (var match in matches)
                {
                    match.GetOutcome(v, ref points, ref gf, ref ga, ref wins, ref draws, ref losses);
                }

                table.Add(new TableItem(v, new int[] { matches.Count, wins, draws, losses, gf, ga, gf - ga, points }));

            }
            return SortList(table);
        }



        List<string> GetTeamList(List<Fixture> results)
        {
            List<string> participants = new List<string>();
            foreach (var v in results)
            {
                bool hAdded = false;
                bool aAdded = false;

                foreach (var participant in participants)
                {
                    if (participant == v.HomeTeam)
                    {
                        hAdded = true;
                    }
                    if (participant == v.AwayTeam)
                    {
                        aAdded = true;
                    }
                }

                if (!hAdded)
                {
                    participants.Add(v.HomeTeam);
                }
                if (!aAdded)
                {
                    participants.Add(v.AwayTeam);
                }
            }
            return participants;
        }


        // replace all int[] with TableItem class
        List<TableItem> SortList(List<TableItem> scores)
        {
            scores.Sort((s2, s1) =>
            {
                int s1Points = s1.TableData[7];
                int s2Points = s2.TableData[7];
                int s1Gd = s1.TableData[6];
                int s2Gd = s2.TableData[6];
                int s1Gf = s1.TableData[4];
                int s2Gf = s2.TableData[4];


                if (s1Points != s2Points) return s1Points.CompareTo(s2Points);
                if (s1Gd != s2Gd) return s1Gd.CompareTo(s2Gd);
                if (s1Gf != s2Gf) return s1Gd.CompareTo(s2Gf);
                else return s1.Team.CompareTo(s2.Team);
            });

            return scores;
        }


        public object[] GetContextTable(string team, List<Fixture> results, TableType tableType)
        {
            var fullTable = GetTable(results, tableType);

            var returnList = new List<TableItem>();

            int currIndex = 0;
            foreach(var item in fullTable)
            {
                if(item.Team == team)
                {
                    break;
                }
                currIndex++;
            }

            var startIndex = 0;

            if(currIndex > 2)
            {
                startIndex = currIndex - 2;
            }
            if(currIndex > fullTable.Count - 3)
            {
                startIndex = fullTable.Count - 5;
            }
            
            for (int i = startIndex; i < startIndex + 5; i++)
            {
                returnList.Add(fullTable[i]);
            }

            return new object[] { returnList, startIndex };
        }


    }
}
