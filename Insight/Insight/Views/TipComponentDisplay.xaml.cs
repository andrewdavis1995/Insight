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
using Insight.Models;
using Insight.UI_Fetcher;

namespace Insight.Views
{
    /// <summary>
    /// Interaction logic for TipComponentDisplay.xaml
    /// </summary>
    public partial class TipComponentDisplay : UserControl
    {
        public TipComponentDisplay()
        {
            InitializeComponent();
        }

        public TipComponentDisplay(PossibleBet item)
        {
            InitializeComponent();

            imgHome.Source = new BadgeFetcher().GetBadge(item.HomeTeam);
            txtWhat.Text = item.GetOutput();
            txtDescription.Text = item.HomeTeam + " vs. " + item.AwayTeam;

            if (item.Type == BetType.BTTS || item.Type == BetType.Over1AndAHalf)
            {
                imgAway.Source = new BadgeFetcher().GetBadge(item.AwayTeam);
            }
            else
            {
                imgAway.Source = null;
                if(item.Type == BetType.AwayWin)
                {
                    imgHome.Source = new BadgeFetcher().GetBadge(item.AwayTeam);
                }
            }
        }
    }
}
