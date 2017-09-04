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
        public delegate void UpdateProgressDelegate(Label label, int percentage);
        public delegate void ButtonShower();

        public LoadingScreen()
        {
            InitializeComponent();
            LoadData();
            //thr.Join();

            // DONE
            //var mw = new MainWindow();
            //this.Hide();
            //mw.Show();
        } 
        

        /// <summary>
        /// Function to be called by delegate - update appearance
        /// </summary>
        /// <param name="label">Label to change height of</param>
        /// <param name="val">Percentage of loading complete</param>
        public void DisplayProgressBar(Label label, int val)
        {
            label.Dispatcher.Invoke(new UpdateProgressDelegate(MoveBar), label, val);
        }

        /// <summary>
        /// Function for dispatcher to call
        /// </summary>
        /// <param name="label">Label to change height of</param>
        /// <param name="val">Percentage/height of the label</param>
        void MoveBar(Label label, int val)
        {
            // if the balue is less than 0, make it 0 - error catching
            if (val < 0) { val = 0; }

            label.Width = val;
        }

        void StartProcess()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // store list of all threads
            List<Thread> thrPool = new List<Thread>();
            LeagueHolder.LeagueList.Add("Premier League", new League(DisplayProgressBar, OverlayPremiership, "Premier League", "premier-league", 20));
            LeagueHolder.LeagueList.Add("Championship", new League(DisplayProgressBar, OverlayChampionship, "Championship", "championship", 24));
            LeagueHolder.LeagueList.Add("League One", new League(DisplayProgressBar, OverlayLeagueOne, "League One", "league-1", 24));
            LeagueHolder.LeagueList.Add("League Two", new League(DisplayProgressBar, OverlayLeagueTwo, "League Two", "league-2", 24));
            LeagueHolder.LeagueList.Add("Scottish Premier League", new League(DisplayProgressBar, OverlayScottishPremiership, "Scottish Premier League", "scottish-premier", 12));
            LeagueHolder.LeagueList.Add("Scottish Championship", new League(DisplayProgressBar, OverlayScottishChampionship, "Scottish Championship", "scottish-championship", 10));
            
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

            var sff = LeagueHolder.LeagueList["Premier League"];
            cmdContinue.Dispatcher.Invoke(new ButtonShower(ShowContinueButton));

        }

        void ShowContinueButton()
        {
            cmdContinue.Visibility = Visibility.Visible;
        }

        private void LoadData()
        {
            Thread thr = new Thread(StartProcess);
            thr.SetApartmentState(ApartmentState.STA);
            thr.Start();
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Hide();
        }
    }
}
