using DT.iOS.DatePickerDialog;
using Foundation;
using PMVOnline.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace PMVOnline.iOS.Services
{
    public class DateTimeService : IDateTimeService
    {
        public TaskCompletionSource<DateTime?> datePickerSource;
        public TaskCompletionSource<(int, int)> timePickerSource;
        DatePickerDialog dialog;

        //public Task<DateTime?> PickDateAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null)
        //{
        //    if (datePickerSource != null && !datePickerSource.Task.IsCompleted)
        //    {
        //        return datePickerSource.Task;
        //    }
        //    datePickerSource = new TaskCompletionSource<DateTime?>();
        //    if (dialog == null) dialog = new DatePickerDialog();
        //    dialog.Show(LocalizedResourceHelper.GetText("dateTimePicker_lblPickDateTitle"), LocalizedResourceHelper.GetText("dateTimePicker_btnOk"), LocalizedResourceHelper.GetText("dateTimePicker_btnCancel"), UIDatePickerMode.Date, (dt) =>
        //    {
        //        datePickerSource.TrySetResult(dt.Date);
        //    }, current == null ? DateTime.Now : (System.DateTime)current, max, min, () => datePickerSource.TrySetResult(null));
        //    return datePickerSource.Task;
        //}

        //public Task<(int, int)> PickTimeAsync()
        //{
        //    if (timePickerSource != null && !timePickerSource.Task.IsCompleted)
        //    {
        //        return timePickerSource.Task;
        //    }
        //    timePickerSource = new TaskCompletionSource<(int, int)>();
        //    if (dialog == null) dialog = new DatePickerDialog();
        //    dialog.Show(LocalizedResourceHelper.GetText("dateTimePicker_lblPickTimeTitle"), LocalizedResourceHelper.GetText("dateTimePicker_btnOk"), LocalizedResourceHelper.GetText("dateTimePicker_btnCancel"), UIDatePickerMode.Time, (dt) =>
        //    {
        //        timePickerSource.TrySetResult((dt.Hour, dt.Minute));
        //    }, DateTime.Now, null, null, () => timePickerSource.TrySetResult((12, 0)));
        //    return timePickerSource.Task;
        //}

        public Task<DateTime?> PickDateTimeAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null)
        {
            if (datePickerSource != null && !datePickerSource.Task.IsCompleted)
            {
                return datePickerSource.Task;
            }
            datePickerSource = new TaskCompletionSource<DateTime?>();
            if (dialog == null) dialog = new DatePickerDialog();
            dialog.Show("Chọn Ngày", "Chọn", "Hủy", UIDatePickerMode.DateAndTime, (dt) =>
            {
                datePickerSource.TrySetResult(dt.Date);
            }, current == null ? DateTime.Now : (System.DateTime)current, max, min, () => datePickerSource.TrySetResult(null));
            return datePickerSource.Task;
        }

        public Task<DateTime?> PickDateAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null)
        {
            throw new NotImplementedException();
        }

        public Task<(int, int)> PickTimeAsync()
        {
            throw new NotImplementedException();
        }

        ~DateTimeService()
        {
            dialog?.Dispose();
            dialog = null;
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        dialog?.Dispose();
        //        dialog = null;
        //    }
        //    base.Dispose(disposing);
        //}
    }
}