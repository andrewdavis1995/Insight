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
    /// Interaction logic for BetTypeSelection.xaml
    /// </summary>
    public partial class BetTypeSelection : UserControl
    {
        public BetTypeSelection()
        {
            InitializeComponent();
        }

        public void SetValues(string name, string description)
        {
            txtName.Content = name;
            txtDescription.Content = description;
        }
    }
}
