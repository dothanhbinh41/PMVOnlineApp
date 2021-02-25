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
                    return "00:00";
                }
                if (remaining.TotalDays >= 1)
                {
                    return $"{remaining.Days}N {remaining.Hours}:{remaining.Minutes}";
                }
                return $"{remaining.Hours}:{remaining.Minutes}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
