using Insight.Converters;
using Insight.Holders;
using Insight.Models;
using Insight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Insight
{
    /// <summary>
    /// Interaction logic for FixturePage.xaml
    /// </summary>
    public partial class FixturePage : Window
    {
        public FixturePage()
        {
            InitializeComponent();
            calenderView.FixturePageItem = this;
            calenderView.Initialise(DateTime.Now.Month, DateTime.Now.Year);

            CalendarView.Day = DateTime.Now.Day;
            CalendarView.Month = DateTime.Now.Month;
            CalendarView.Year = DateTime.Now.Year;

            AddMatches();

        }

        public void AddMatches()
        {
            var today = new DateTime(CalendarView.Year, CalendarView.Month, CalendarView.Day);
            var list = new List<Fixture>();

            foreach (var league in LeagueHolder.LeagueList)
            {
                for(var i = 0; i < league.Value.Fixtures.Count; i++)
                {
                    var fixture = league.Value.Fixtures[i];

                    var day = new DateConverter().GetDateTimeFromString(fixture.KickOff);
                    if(day.Date == today.Date)
                    {
                        list.Add(fixture);
                    }

                    if (day.Date > today.Date)
                    {
                        break;
                    }

                    if (today.Date - day.Date > new TimeSpan(28, 0, 0, 0))
                    {
                        i += league.Value.NumTeams*2;
                    }
                }
            }
            
            var grouped = list.GroupBy(res => res.League);

            // sort by time

            fixtureStack.Children.Clear();

            foreach(var item in grouped)
            {
                var lblTitle = new Label();
                lblTitle.Content = item.Key;
                lblTitle.FontFamily = new FontFamily("Abel");
                lblTitle.FontSize = 12;
                lblTitle.Foreground = new SolidColorBrush(Color.FromRgb(40, 40, 40));
                lblTitle.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                lblTitle.Padding = new Thickness(20, 0, 0, 0);
                lblTitle.HorizontalAlignment = HorizontalAlignment.Stretch;
                lblTitle.VerticalContentAlignment = VerticalAlignment.Center;
                lblTitle.Height = 30;
                fixtureStack.Children.Add(lblTitle);

                var sorted = item.OrderBy(res => res.KickOff.Split('@')[1].Split(':')[0]).ThenBy(res => res.KickOff.Split('@')[1].Split(':')[1]);

                foreach (var fixt in sorted)
                {
                    var obj = new FixtureViewLarge();
                    obj.fixtureView.txtHomeTeam.Text = fixt.HomeTeam;
                    obj.fixtureView.txtAwayTeam.Text = fixt.AwayTeam;
                    obj.fixtureView.txtScore.Text = " vs. ";
                    obj.fixtureView.txtScore.FontSize = 12;
                    obj.fixtureView.txtKickoff.Text = fixt.KickOff.Split('@')[1].Replace('£', ':');
                    fixtureStack.Children.Add(obj);
                }


                // add all items

            }

            SelectedDate.Text = today.ToString("dd MMMM yyyy");

        }


        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

    }
}
