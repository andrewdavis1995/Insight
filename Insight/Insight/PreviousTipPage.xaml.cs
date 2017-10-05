using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PreviousTipPage.xaml
    /// </summary>
    public partial class PreviousTipPage : Window
    {
        public PreviousTipPage()
        {
            InitializeComponent();
            LoadResults();
        }

        void LoadResults()
        {
            var dirPath = Environment.CurrentDirectory + "\\Tips\\History";
            var files = Directory.GetFiles(dirPath);

            var totalWins = 0d;
            var totalStake = 0d;

            foreach(var file in files)
            {
                var sr = new StreamReader(file);

                var line = sr.ReadLine();

                while(line != null)
                {
                    var split = line.Split('@');

                    var type = split[0];
                    var date = split[1];
                    var time = split[2];
                    var homeTeam = split[3];
                    var awayTeam = split[4];
                    var player = split[5];
                    var odds = split[6];
                    var confidence = split[7];
                    var won = split[8];
                    
                    var lbl = new Label();
                    lbl.Content = type + ", " + homeTeam + " vs. " + awayTeam;
                    lbl.Content += player == "" ? "" : " (" + player + ")";
                    lbl.Content += " - " + won;

                    if (bool.Parse(won))
                    {
                        // calculate winnings
                        var oddSplit = odds.Split('/');
                        var doub = double.Parse(oddSplit[0]) / double.Parse(oddSplit[1]);
                        
                        var returns = 1 + (1 * doub);

                        totalWins += returns;

                        lbl.Content += " --> £" + Math.Round(returns, 2);
                        lbl.Background = new SolidColorBrush(Color.FromArgb(220, 66, 190, 70));
                    }else
                    {
                        lbl.Background = new SolidColorBrush(Color.FromArgb(140, 120, 44, 55));
                    }

                    ResultStack.Children.Add(lbl);

                    totalStake += 1;

                    line = sr.ReadLine();
                }

                sr.Close();
            }

            MessageBox.Show("£" + Math.Round(totalWins, 2) + " from £" + Math.Round(totalStake, 2));

        }
        
        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

    }
}
