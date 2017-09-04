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
    /// Interaction logic for FixtureView.xaml
    /// </summary>
    public partial class FixtureView : UserControl
    {
        public FixtureView()
        {
            InitializeComponent();
        }

        public void SetUpForLargeView()
        {
            txtHomeTeam.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            txtAwayTeam.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            txtScore.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            txtKickoff.Foreground = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));

            txtHomeTeam.FontSize += 2;
            txtAwayTeam.FontSize += 2;
            txtScore.FontSize += 5;
        }

    }
}
