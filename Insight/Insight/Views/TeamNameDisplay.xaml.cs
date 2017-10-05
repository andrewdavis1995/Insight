using Insight.UI_Fetcher;
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
    /// Interaction logic for TeamNameDisplay.xaml
    /// </summary>
    public partial class TeamNameDisplay : UserControl
    {
        public TeamNameDisplay()
        {
            InitializeComponent();
        }
        public TeamNameDisplay(string team)
        {
            InitializeComponent();

            txtTeam.Text = team;
            imgBadge.Source = new BadgeFetcher().GetBadge(team);
        }
    }
}
