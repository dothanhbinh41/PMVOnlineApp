using Foundation;
using PMVOnline.Accounts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace PMVOnline.iOS.Services
{
    public class OpenSettingService : IOpenSettingService
    {
        public void OpenSetting()
        {
            var settingsString = UIKit.UIApplication.OpenSettingsUrlString;
            var url = new NSUrl(settingsString);
            UIApplication.SharedApplication.OpenUrl(url); 
        }
    }
}