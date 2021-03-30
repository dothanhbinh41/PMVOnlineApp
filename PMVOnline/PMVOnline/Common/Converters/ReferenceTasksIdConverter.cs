using PMVOnline.Tasks.Models;
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
            if (value is IList<TaskModel> ids)
            {
                if (ids.Count == 0)
                {
                    return string.Empty;
                }
                if (ids.Count == 1)
                {
                    return $"#{ids[0].Id}";
                }
                return $"({ids.Count}) {ids.Select(d => $"#{d.Id}").Aggregate((d1, d2) => d1 + " " + d2)}";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
