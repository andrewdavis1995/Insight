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
        #region Variables for tracking the loading
        const int BETS_LOADING_TIME_WEIGHT = 35;
        const int TABLE_LOADING_TIME_WEIGHT = 10;
        const int FIXTURES_LOADING_TIME_WEIGHT = 20;
        const int RESULTS_LOADING_TIME_WEIGHT = 35; // these should add up to the height of the overlay label
        // How much progress is left - give each process a different weighting in constants above
        int RemainingProgress = 100;
        // callbacks and labels required
        Action<Label, int> callback;
        Label label;
        #endregion

        #region Attributes
        public string LeagueName { get; }
        public string SkySportsName { get; }
        public int NumTeams { get; }
        public List<TableItem> Table { get; set; } = new List<TableItem>();
        public List<Fixture> Fixtures { get; set; } = new List<Fixture>();
        public List<Fixture> Results { get; set; } = new List<Fixture>();
        public List<string> PossibleBets { get; set; } = new List<string>();
        #endregion


        public League(Action<Label, int> timerCallback, Label lbl, string name, string skyName, int numTeams)
        {
            callback = timerCallback;
            label = lbl;
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
            var links = of.LoadLinks(af.GetAlternativeLeagueName("Premier League"));

            foreach(var link in links)
            {
                of.GetBets(link); // thread this
            }

            RemainingProgress -= BETS_LOADING_TIME_WEIGHT;
            callback(label, RemainingProgress);
        }

        void LoadFixtures()
        {
            var fixtureFetcher = new FixturesFetcher();
            Fixtures = fixtureFetcher.GetFixtures(SkySportsName);

            RemainingProgress -= FIXTURES_LOADING_TIME_WEIGHT;
            callback(label, RemainingProgress);
        }

        void LoadResults()
        {
            var resultsFetcher = new ResultsFetcher();
            Results = resultsFetcher.GetResults(SkySportsName);

            RemainingProgress -= RESULTS_LOADING_TIME_WEIGHT;
            callback(label, RemainingProgress);

            Table = new StatFetcher().GetTable(this.Results, TableType.FullTable);
            RemainingProgress -= TABLE_LOADING_TIME_WEIGHT;
            callback(label, RemainingProgress);

        }
        
    }
}
