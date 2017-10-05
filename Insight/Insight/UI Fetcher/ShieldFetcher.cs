using Insight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Insight.UI_Fetcher
{
    public class ShieldFetcher
    {
        public BitmapImage GetShield(string classified)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();

            var uri = new Uri(@"/Insight;component/Resources/value.png", UriKind.Relative);
                        
            switch (classified)
            {
                case "Long Shot":
                    uri = new Uri(@"/Insight;component/Resources/longshot.png", UriKind.Relative);
                    break;
                case "Accumulator":
                case "Double":
                    uri = new Uri(@"/Insight;component/Resources/acca.png", UriKind.Relative);
                    break;
                case "Goals Acca":
                    uri = new Uri(@"/Insight;component/Resources/over1.png", UriKind.Relative);
                    break;
                case "BTTS Acca":
                    uri = new Uri(@"/Insight;component/Resources/over1.png", UriKind.Relative);
                    break;
                default:
                    uri = new Uri(@"/Insight;component/Resources/value.png", UriKind.Relative);
                    break;
            }

            if (classified.Contains(" to score"))
            {
                uri = new Uri(@"/Insight;component/Resources/scorer.png", UriKind.Relative);
            }

            logo.UriSource = uri;
            logo.EndInit();

            return logo;



        }
    }
}
