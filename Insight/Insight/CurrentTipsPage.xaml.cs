using Insight.Models;
using Insight.Views;
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

namespace Insight
{
    /// <summary>
    /// Interaction logic for CurrentTipsPage.xaml
    /// </summary>
    public partial class CurrentTipsPage : Window
    {
        private List<PossibleBet> selections;

        public CurrentTipsPage()
        {
            InitializeComponent();
        }

        public CurrentTipsPage(List<PossibleBet> selections)
        {
            this.selections = selections;
            InitializeComponent();
            ShowTips();
        }

        string GetBetTypeString(BetType type)
        {
            switch (type)
            {
                case BetType.HomeWin: return "Home Win";
                case BetType.AwayWin: return "Away Win";
                case BetType.BTTS: return "Both Teams To Score";
                case BetType.Over1AndAHalf: return "Over 1.5 Goals";
                case BetType.Draw: return "Draw";
                case BetType.ToScoreIn90: return "Player to score";
                default: return "Bet";
            }
        }

        private void ShowTips()
        {
            selections = selections.OrderByDescending(s => s.Confidence).ToList();

            for(var i = 0; i < selections.Count; i++)
            {
                var tipView = new TipView();
                tipView.lblBetType.Text = GetBetTypeString(selections[i].Type);
                tipView.lblDate.Text = selections[i].When.Split('@')[0].ToString();
                tipView.lblOdds.Text = selections[i].Odds;
                tipView.Tag = i.ToString();
                tipView.Height = 250;
                tipView.MouseDown += TipView_MouseDown;
                tipView.lblPlayer.Text = selections[i].Player;
                tipView.lblProgress.Width = (selections[i].Confidence / 100) * 240;
                tipView.lblFixture.Text = selections[i].HomeTeam + " vs. " + selections[i].AwayTeam;
                Grid.SetColumn(tipView, (i % 5) + 1);
                Grid.SetRow(tipView, (i / 5) + 1);
                TipsGrid.Children.Add(tipView);
            }
        }

        private void TipView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var tag = ((TipView)sender).Tag.ToString();
            var id = int.Parse(tag);

            var tip = selections[id];

            FixturePopup.Visibility = Visibility.Visible;
            lblFixture.Content = tip.HomeTeam + " vs. " + tip.AwayTeam;
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            FixturePopup.Visibility = Visibility.Hidden;
        }
    }
}
