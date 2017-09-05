using Insight.Data_Fetchers;
using Insight.Holders;
using Insight.Models;
using Insight.UI_Fetcher;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Insight.Views
{
    /// <summary>
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        public delegate void MoveToNextPageDelegate();

        public LoadingScreen()
        {
            InitializeComponent();
            LoadData();
        } 
                
        void StartProcess()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // store list of all threads
            List<Thread> thrPool = new List<Thread>();
            LeagueHolder.LeagueList.Add("Premier League", new League("Premier League", "premier-league", 20));
            LeagueHolder.LeagueList.Add("Championship", new League("Championship", "championship", 24));
            LeagueHolder.LeagueList.Add("League One", new League("League One", "league-1", 24));
            LeagueHolder.LeagueList.Add("League Two", new League("League Two", "league-2", 24));
            LeagueHolder.LeagueList.Add("Scottish Premier League", new League("Scottish Premier League", "scottish-premier", 12));
            LeagueHolder.LeagueList.Add("Scottish Championship", new League("Scottish Championship", "scottish-championship", 10));
            
            // start a thread for second box, and add to list
            foreach (var child in LeagueHolder.LeagueList)
            {
                var thr = new Thread(child.Value.LoadData);
                thr.Start();
                thrPool.Add(thr);
            }

            // wait until all threads complete
            while (thrPool.Any(t => t.IsAlive)) { }

            // could maybe remove this line
            while (LeagueHolder.LeagueList.Any(l => l.Value.Results.Any(r => !r.LoadingComplete))) { }

            sw.Stop();

            var v = sw.ElapsedMilliseconds;

            this.Dispatcher.Invoke(new MoveToNextPageDelegate(Continue));

        }
        

        private void LoadData()
        {
            Thread thr = new Thread(StartProcess);
            thr.SetApartmentState(ApartmentState.STA);
            thr.Start();
        }

        private void Continue()
        {
            new MainWindow().Show();
            this.Hide();
        }
    }
}
