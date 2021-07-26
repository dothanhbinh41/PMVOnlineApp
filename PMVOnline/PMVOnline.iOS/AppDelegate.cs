using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using PMVOnline.Common.Services;
using PMVOnline.iOS.Services;
using Prism;
using Prism.Ioc;
using UIKit;
using UserNotifications;

namespace PMVOnline.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new IOSPlatform()));
            Init(app, options);
            return base.FinishedLaunching(app, options);
        }

        void Init(UIApplication app, NSDictionary options)
        {  
            Xamarin.Forms.FormsMaterial.Init();

            FirebasePushNotificationManager.Initialize(options, true);
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert;
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            CachedImageRenderer.Init(); 
            Xamarin.Essentials.Platform.GetCurrentUIViewController(); 
            Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();  
            Firebase.Core.App.Configure();  
        }

        public class IOSPlatform : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.Register<IDateTimeService, DateTimeService>();
            }
        }
    }
}
