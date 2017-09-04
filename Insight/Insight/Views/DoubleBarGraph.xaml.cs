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
    /// Interaction logic for DoubleBarGraph.xaml
    /// </summary>
    public partial class DoubleBarGraph : UserControl
    {
        public DoubleBarGraph()
        {
            InitializeComponent();
        }

        public void UpdateValues(int[][] values, int total)
        {
            var children = mainGrid.Children;

            int incorrectTypes = 0;

            for (int i = 0; i < children.Count; i++)
            {
                try
                {
                    var obj = children[i] as BarChartItem;

                    var percGreen = ((float)values[i-incorrectTypes][0] / total) * 160;
                    var percRed = ((float)values[i-incorrectTypes][1] / total) * 160;

                    if (Single.IsNaN(percGreen)) { percGreen = 0; }
                    if (Single.IsNaN(percRed)) { percRed = 0; }

                    obj.GreenSection.Height = percGreen;
                    obj.RedSection.Height = percRed;
                }
                catch (Exception e)
                {
                    incorrectTypes++;
                }
            }
        }
    }
}
