using Insight.Holders;
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
    /// Interaction logic for LeagueSelection.xaml
    /// </summary>
    public partial class LeagueSelection : Window
    {
        public LeagueSelection()
        {
            InitializeComponent();
        }

        private void LeagueClicked(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            var tag = grid.Tag.ToString();

            var league = LeagueHolder.LeagueList[tag];
            
            LeagueView lv = new LeagueView(league);
            lv.Owner = this;
            lv.Show();

        }
        
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

    }
}
