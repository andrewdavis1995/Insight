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
    public partial class SliderBarWithToggle : UserControl
    {
        public SliderBarWithToggle()
        {
            InitializeComponent();
        }

        private void SliderMoved(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TheSlider.lblValue.Text = Math.Round(TheSlider.TheSlider.Value).ToString();
        }

        private void rdBtn1_Checked(object sender, RoutedEventArgs e)
        {
            TheSlider.Visibility = Visibility.Collapsed;
        }

        private void rdBtn2_Checked(object sender, RoutedEventArgs e)
        {
            TheSlider.Visibility = Visibility.Visible;
        }
        public void SetValues(int min, int max, int step, string title, int current, string title1, string title2)
        {
            TheSlider.TheSlider.Minimum = 0;
            TheSlider.TheSlider.Maximum = 100;
            TheSlider.TheSlider.TickFrequency = step;
            TheSlider.TheSlider.Value = current;
            TheSlider.lblTitle.Text = title;
            rdBtn1.Content = title1;
            rdBtn2.Content = title2;
            TheSlider.lblMaxVal.Text = max.ToString();
            TheSlider.lblMinVal.Text = min.ToString();
            TheSlider.lblValue.Text = current.ToString();
            TheSlider.TheSlider.ValueChanged += SliderMoved;
        }
    }
}