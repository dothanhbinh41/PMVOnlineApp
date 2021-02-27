using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PMVOnline.Common.Converters
{
    public class ReferenceTasksIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long[] ids)
            {
                return $"({ids.Length}) {ids.Select(d=>$"#{d}").Aggregate((d1,d2)=>d1+" "+d2)}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
