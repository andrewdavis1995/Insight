using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.Converters
{
    public class DateConverter
    {
        public DateTime GetDateTimeFromString(string date)
        {
            // dddd = Monday, Tuesday etc.
            // MMMM = January, February etc.
            // d = date of month (1, 2, 3 etc.)

            date.Replace("st ", "");
            date.Replace("nd ", "");
            date.Replace("rd ", "");

            var dateTime = DateTime.ParseExact(date, "dddd d MMMM", System.Globalization.CultureInfo.InvariantCulture);
            //var datee = DateTime.Parse(date + " " + DateTime.Now.Year);
            //return datee;
            return dateTime;
        }
    }
}