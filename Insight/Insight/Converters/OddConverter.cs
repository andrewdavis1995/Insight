using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Converters
{
    class OddConverter
    {
        public string DoubleToFraction(double odds)
        {
            odds = Math.Round(odds, 1);

            var numerator = odds * 100;
            var denominator = 100d;

            var changed = false;

            do
            {
                changed = false;

                for (var i = 10; i > 1; i--)
                {
                    if (numerator / i == (int)(numerator / i) && denominator / i == (int)(denominator / i))
                    {
                        numerator /= i;
                        denominator /= i;
                        changed = true;
                        break;
                    }
                }
            } while (changed);

            return numerator + "/" + denominator;

        }
    }
}
