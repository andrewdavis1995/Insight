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
using System.Windows.Shapes;
using Insight.Models;
using Insight.Converters;
using Insight.Views;
using Insight.UI_Fetcher;

namespace Insight
{
    /// <summary>
    /// Interaction logic for SelectionScreen.xaml
    /// </summary>
    public partial class SelectionScreen : Window
    {
        private readonly List<Selection> selections;
        int _dateIndex = 0;
        List<bool> selected = new List<bool>();
        DateTime dateTime = new DateTime();
        
        public SelectionScreen(List<Selection> selections)
        {
            InitializeComponent();
            this.selections = selections;
            foreach(var v in selections) { selected.Add(false); }

            dateTime = DateTime.Now;
            lblDate.Content = dateTime.ToString("dddd dd/MM/yyyy");

            DisplayTips();
        }


        public Selection GetItemToShow(List<Selection> list)
        {
            Selection item = null;

            for(var v = 0; v < list.Count; v++)
            {
                if((list[v].Items[0].Type == BetType.HomeWin || list[v].Items[0].Type == BetType.AwayWin) && list[v].Items.Count > 1 && !selected[v])
                {
                    item = list[v];
                    selected[v] = true;
                    break;
                }
            }
            return item;
        }
        public Selection GetItemToShowNext(List<Selection> list)
        {
            Selection item = null;

            for (var v = 0; v < list.Count; v++)
            {
                if ((list[v].Items[0].Type == BetType.HomeWin || list[v].Items[0].Type == BetType.AwayWin) && !selected[v])
                {
                    item = list[v];
                    selected[v] = true;
                    break;
                }
            }
            return item;
        }

        public Selection GetNext(List<Selection> list)
        {
            for(var i = 0; i < list.Count; i++)
            {
                if(!selected[i]) { selected[i] = true; return list[i]; }
            }
            return null;
        }


        private void DisplayTips()
        {
            var displayers = new List<Selection>();


            var dateParser = new DateConverter();

            foreach(var bet in selections)
            {
                var date = dateParser.GetDateTimeFromString(bet.Date);
                if(date.Date == dateTime.Date)
                {
                    displayers.Add(bet);
                }
            }

            selected = new List<bool>();
            foreach (var v in displayers) { selected.Add(false); }

            var topItem = GetItemToShow(displayers);
            var secondItem = GetItemToShow(displayers);
            var thirdItem = GetItemToShow(displayers);

            if(topItem == null) { topItem = GetItemToShowNext(displayers); }
            if(secondItem == null) { secondItem = GetItemToShowNext(displayers); }
            if(thirdItem == null) { thirdItem = GetItemToShowNext(displayers); }

            try
            {
                if (topItem == null) { topItem = GetNext(displayers); }
                if (secondItem == null) { secondItem = GetNext(displayers); }
                if (thirdItem == null) { thirdItem = GetNext(displayers); }
            }
            catch (Exception) { }

            var fetcher = new ShieldFetcher();


            if (topItem != null)
            {
                TipDisplay1.Visibility = Visibility.Visible;
                TipDisplay1.txtTitle.Text = topItem.Name;
                TipDisplay1.txtOdds.Text = "£1 returns £" + topItem.GetOverallOdds();
                TipDisplay1.txtTime.Text = topItem.Date.Split('@')[1];
                TipDisplay1.imgShield.Source = fetcher.GetShield(topItem.Name);
                TipDisplay1.AddItems(topItem);
            }
            else
            {
                TipDisplay1.Visibility = Visibility.Collapsed;
            }
             
            if (secondItem != null)
            {
                TipDisplay2.Visibility = Visibility.Visible;
                TipDisplay2.txtTitle.Text = secondItem.Name;
                TipDisplay2.txtOdds.Text = "£1 returns £" + secondItem.GetOverallOdds();
                TipDisplay2.txtTime.Text = secondItem.Date.Split('@')[1];
                TipDisplay2.imgShield.Source = fetcher.GetShield(secondItem.Name);
                TipDisplay2.AddItems(secondItem);
            }
            else
            {
                TipDisplay2.Visibility = Visibility.Collapsed;
            }

            if (thirdItem != null)
            {
                TipDisplay3.Visibility = Visibility.Visible;
                TipDisplay3.txtTitle.Text = thirdItem.Name;
                TipDisplay3.txtOdds.Text = "£1 returns £" + thirdItem.GetOverallOdds();
                TipDisplay3.txtTime.Text = thirdItem.Date.Split('@')[1];
                TipDisplay3.imgShield.Source = fetcher.GetShield(thirdItem.Name);
                TipDisplay3.AddItems(thirdItem);
            }
            else
            {
                TipDisplay3.Visibility = Visibility.Collapsed;
            }

            stckOtherTips.Children.Clear();

            var next = GetNext(displayers);

            while (next != null)
            {
                var display = new TipDisplaySmall(next);
                display.txtTitle.Text = next.Name;
                display.txtOdds.Text = "£1 returns £" + next.GetOverallOdds();
                display.imgIcon.Source = fetcher.GetShield(next.Name);
                stckOtherTips.Children.Add(display);
                next = GetNext(displayers);
            }

        }

        private void cmdLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_dateIndex > 0)
            {
                _dateIndex--;
            }

            dateTime = DateTime.Now.AddDays(_dateIndex);
            lblDate.Content = dateTime.ToString("dddd dd/MM/yyyy");
            DisplayTips();
        }

        private void cmdRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_dateIndex <= 15)
            {
                _dateIndex++;
            }

            dateTime = DateTime.Now.AddDays(_dateIndex);
            lblDate.Content = dateTime.ToString("dddd dd/MM/yyyy");
            DisplayTips();
        }
    }
}
