using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PMVOnline.Accounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMVOnline.Droid.Services
{
    public class OpenSettingService : IOpenSettingService
    {
        public void OpenSetting()
        {
            var intent = new Intent(Android.Provider.Settings.ActionDisplaySettings);
            intent.SetFlags(ActivityFlags.NewTask);
            Xamarin.Essentials.Platform.CurrentActivity.StartActivity(intent);
        }
    }
}