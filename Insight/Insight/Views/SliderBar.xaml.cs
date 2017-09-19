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
    /// Interaction logic for SliderBar.xaml
    /// </summary>
    public partial class SliderBar : UserControl
    {
        public SliderBar()
        {
            InitializeComponent();
        }

        public void SetValues(float min, float max, float step, string title, float current)
        {
            TheSlider.Minimum = min;
            TheSlider.Maximum = max;
            TheSlider.TickFrequency = step;
            TheSlider.Value = current;
            lblTitle.Text = title;
            lblMaxVal.Text = max.ToString();
            lblMinVal.Text = min.ToString();
            lblValue.Text = current.ToString();
            TheSlider.ValueChanged += SliderMoved;
        }

        private void SliderMoved(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblValue.Text = Math.Round(TheSlider.Value, 1).ToString();
        }
    }
}