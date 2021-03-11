using PMVOnline.Api.Dtos.Tasks;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PMVOnline.Common.Converters
{
    public class TaskStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is TaskStatus stt)
            {
                switch (stt)
                {
                    case TaskStatus.Approved:
                        return "Duyệt";
                    case TaskStatus.Completed:
                        return "Hoàn thành";
                    case TaskStatus.Incompleted:
                        return "Không hoàn thành";
                    case TaskStatus.Pending:
                        return "Chờ duyệt";
                    case TaskStatus.Rejected:
                    default:
                        return "Không duyệt";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
