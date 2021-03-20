using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PMVOnline.Common.Converters
{
    public class RemainingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
            {
                var remaining = dt.Subtract(DateTime.Now);
                if (remaining < TimeSpan.Zero)
                {
                    return "Quá hạn";
                }
                if (remaining.TotalDays >= 1)
                {
                    return $"{remaining.Days}N {remaining.Hours}h{remaining.Minutes}";
                }
                return $"{remaining.Hours}h{remaining.Minutes}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
