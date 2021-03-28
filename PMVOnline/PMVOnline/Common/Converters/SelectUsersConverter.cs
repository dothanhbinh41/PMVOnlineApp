using PMVOnline.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PMVOnline.Common.Converters
{
    public class SelectUsersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<UserModel> lst && lst.Count > 0)
            {
                if (lst.Count == 1)
                {
                    return lst.FirstOrDefault().FullName;
                }
                return lst.Select(d => d.FullName).Aggregate((d1, d2) => d1 + "," + d2);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
