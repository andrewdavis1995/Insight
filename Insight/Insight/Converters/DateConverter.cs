using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Insight.Converters
{
    public class DateConverter
    {
        public DateTime GetDateTimeFromString(string date)
        {
            try
            {
                // remove ordinals
                var str = Regex.Replace(date, @"(?<=[0-9])(?:st|nd|rd|th)", "");

                // remove day name
                var fullStr = "";
                var split = str.Split(' ');
                foreach (var s in split)
                {
                    if (!s.Contains("day")) { fullStr += s + " "; }
                }

                var strSplit = fullStr.Split('@');

                DateTime dt = new DateTime();

                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "dd MMMM", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }
                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "d MMMM", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }
                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "dd MMMM yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }
                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "d MMMM yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }

                // backups - shouldn't be called
                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "MMMM dd yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }
                try
                {
                    dt = DateTime.ParseExact(strSplit[0].Trim(), "MMMM d yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) { }

                var timeSplit = strSplit[1].Split('-');
                if (timeSplit.Length < 2)
                {
                    timeSplit = strSplit[1].Split(':');
                }
                var hour = int.Parse(timeSplit[0]);
                var minute = int.Parse(timeSplit[1]);

                dt = dt.AddHours(hour);
                dt = dt.AddMinutes(minute);

                return dt;

            }
            catch (Exception e)
            {
                return new DateTime();
            }
        }

    }
}