using Insight.Data_Fetchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Insight.Models
{

    public enum TableType
    {
        FullTable, 
        HomeTable, 
        AwayTable
    }


    public class League
    {
        #region Attributes
        public string LeagueName { get; }
        public string SkySportsName { get; }
        public int NumTeams { get; }
        public List<TableItem> Table { get; set; } = new List<TableItem>();
        public List<Fixture> Fixtures { get; set; } = new List<Fixture>();
        public List<Fixture> Results { get; set; } = new List<Fixture>();
        public List<PossibleBet> PossibleBets { get; set; } = new List<PossibleBet>();
        #endregion
        
        public League(string name, string skyName, int numTeams)
        {
            PossibleBets.Capacity = 2000;

            LeagueName = name;
            NumTeams = numTeams;
            SkySportsName = skyName;
        }
        
        public void LoadData()
        {
            // give a chance for screen to show
            Thread.Sleep(200);

            // store list of all threads
            List<Thread> thrPool = new List<Thread>();

            // start step 1
            var resultsThread = new Thread(LoadResults);
            resultsThread.Start();
            thrPool.Add(resultsThread);

            // start step 2
            var betThread = new Thread(LoadBets);
            betThread.Start();
            thrPool.Add(betThread);

            // start step 3
            var fixturesThread = new Thread(LoadFixtures);
            fixturesThread.Start();
            thrPool.Add(fixturesThread);


            // wait until all threads
            while (thrPool.Any(t => t.IsAlive)) { }
        }




        // Data gathering steps below
        void LoadBets()
        {
            AliasFetcher af = new AliasFetcher();
            OddsFetcher of = new OddsFetcher();
            var links = of.LoadLinks(af.GetAlternativeLeagueName(LeagueName));
            var threads = new List<Thread>();

            foreach(var link in links)
            {
                var thr = new Thread(() => GetBets(link));
                threads.Add(thr);
                thr.Start();
                //GetBets(link);
            }

            while (threads.Any(thr => thr.IsAlive)) { }
            
        }

        void GetBets(string link)
        {
            OddsFetcher of = new OddsFetcher();
            var bets = of.GetBets(link); // thread this
            PossibleBets.AddRange(bets);
        }


        void LoadFixtures()
        {
            var fixtureFetcher = new FixturesFetcher();
            Fixtures = fixtureFetcher.GetFixtures(SkySportsName);
            
        }

        void LoadResults()
        {
            var resultsFetcher = new ResultsFetcher();
            Results = resultsFetcher.GetResults(SkySportsName);
            
            Table = new StatFetcher().GetTable(this.Results, TableType.FullTable);
        }

        public List<PossibleBet> FindBets(Fixture fixture)
        {
            var bets = new List<PossibleBet>();
            foreach(var bet in PossibleBets)
            {
                if (bet != null)
                {
                    if (bet.HomeTeam == fixture.HomeTeam && bet.AwayTeam == fixture.AwayTeam)
                    {
                        bets.Add(bet);
                    }
                }
            }
            return bets;
        }
        
    }
}
