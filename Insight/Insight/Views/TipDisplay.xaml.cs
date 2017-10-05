using Insight.Models;
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
    /// Interaction logic for TipDisplay.xaml
    /// </summary>
    public partial class TipDisplay : UserControl
    {
        public TipDisplay()
        {
            InitializeComponent();
        }

        public void AddItems(Selection selection)
        {
            TeamStack.Children.Clear();

            if (selection.Items[0].Type == BetType.HomeWin || selection.Items[0].Type == BetType.AwayWin)
            {
                foreach(var item in selection.Items)
                {
                    var team = item.Type == BetType.HomeWin ? item.HomeTeam : item.AwayTeam;
                    var display = new TipComponentDisplay(item);
                    TeamStack.Children.Add(display);
                }
            }
        }

    }
}
