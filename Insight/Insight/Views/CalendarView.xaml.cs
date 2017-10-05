using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Insight.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public static int Month = 1;
        public static int Year = 2017;
        public static int Day = 1;

        public FixturePage FixturePageItem { get; set; }

        public CalendarView()
        {
            InitializeComponent();
        }
        
        public void Initialise(int month, int year)
        {
            ContainerGrid.Children.Clear();

            var daysInMonth = DateTime.DaysInMonth(year, month);

            var now = DateTime.Now;

            int currentRow = 0;

            for (var i = 1; i <= daysInMonth; i++)
            {
                var dateTime = new DateTime(year, month, i);

                CalendarDayView cdv = new CalendarDayView();
                cdv.lblDate.Content = i.ToString();
                cdv.HorizontalAlignment = HorizontalAlignment.Stretch;
                cdv.VerticalAlignment = VerticalAlignment.Stretch;
                cdv.MouseDown += DayClicked;
                cdv.Tag = i.ToString();

                if (dateTime.Date < now.Date)
                {
                    cdv.Background = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
                }

                var dayOfWeek = dateTime.DayOfWeek;

                Grid.SetRow(cdv, currentRow);
                Grid.SetColumn(cdv, (int)dayOfWeek);

                ContainerGrid.Children.Add(cdv);

                if (dayOfWeek == DayOfWeek.Saturday)
                {
                    currentRow++;
                }
            }

            Month = month;
            Year = year;
            lblMonth.Content = new DateTime(year, month, 1).ToString("MMMM");


        }

        private void DayClicked(object sender, MouseButtonEventArgs e)
        {
            var tag = ((CalendarDayView)sender).Tag.ToString();
            Day = int.Parse(tag);
            FixturePageItem.AddMatches();
        }

        private void RightButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            Month++;

            if(Month > 12) { Month = 1; }

            int year;
            if(Month >= 6)
            {
                year = DateTime.Now.Year;
            }
            else
            {
                year = DateTime.Now.Year + 1;
            }

            Initialise(Month, year);
        }

        private void LeftButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            Month--;

            if (Month < 1) { Month = 12; }


            int year;
            if (Month >= 6)
            {
                year = DateTime.Now.Year;
            }
            else
            {
                year = DateTime.Now.Year + 1;
            }

            Initialise(Month, year);

        }
    }
}