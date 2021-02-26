using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PMVOnline.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Droid.Services
{
    public class DateTimeService : Java.Lang.Object, IDateTimeService, DatePickerDialog.IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {

        TaskCompletionSource<DateTime?> datePickerSource;
        TaskCompletionSource<DateTime?> datetimePickerSource;
        TaskCompletionSource<(int, int)> timeSource;

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            if (datePickerSource?.Task.IsCompleted == false)
            {
                datePickerSource.SetResult(view.DateTime);
            }
            datePickerSource = null;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            if (timeSource?.Task.IsCompleted == false)
            {
                timeSource.SetResult((view.Hour, view.Minute));
            }
            timeSource = null;
        }

        public Task<DateTime?> PickDateAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null)
        {
            if (datePickerSource != null && !datePickerSource.Task.IsCompleted)
            {
                return datePickerSource.Task;
            }
            var context = Xamarin.Essentials.Platform.CurrentActivity;
            var currentDate = current ?? DateTime.Now;
            datePickerSource = new TaskCompletionSource<DateTime?>();
            DatePickerDialog dialog = new DatePickerDialog(context, this, currentDate.Year, currentDate.Month, currentDate.Day);
            dialog.DatePicker.DateTime = currentDate;
            if (min != null)
            {
                dialog.DatePicker.MinDate = (long)Math.Round(min.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            }
            if (max != null)
            {
                dialog.DatePicker.MaxDate = (long)Math.Round(max.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            }
            dialog.Show();
            dialog.CancelEvent += (s, e) =>
            {
                datePickerSource.SetResult(null);
            };
            return datePickerSource.Task;
        }

        public async Task<DateTime?> PickDateTimeAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null)
        {
            var date = await PickDateAsync(min, current, max);
            if (!date.HasValue)
            {
                return date;
            }
            var time = await PickTimeAsync(current?.Hour, current?.Minute);
            return date.Value.AddHours(time.Item1).AddMinutes(time.Item2);
        }

        public Task<(int, int)> PickTimeAsync(int? h, int? m)
        {
            if (datePickerSource != null && !datePickerSource.Task.IsCompleted)
            {
                return timeSource.Task;
            }
            var context = Xamarin.Essentials.Platform.CurrentActivity;
            timeSource = new TaskCompletionSource<(int, int)>();

            var hourOfDay = h ?? DateTime.Now.Hour;
            var minutes = h ?? DateTime.Now.Minute;

            TimePickerDialog dialog = new TimePickerDialog(context, this, hourOfDay, minutes, true);
            dialog.Show();
            dialog.CancelEvent += (s, e) =>
            {
                timeSource.SetResult((0, 0));
            };
            return timeSource.Task;
        }
    }
}