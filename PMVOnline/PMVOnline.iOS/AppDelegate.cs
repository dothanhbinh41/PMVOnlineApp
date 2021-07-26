using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
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
            FirebasePushNotificationManager.Initialize(options, new CustomPushHandler());
            return base.FinishedLaunching(app, options);
        }

        void Init(UIApplication app, NSDictionary options)
        {  
            Xamarin.Forms.FormsMaterial.Init();
            Firebase.Core.App.Configure(); 
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert;
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            CachedImageRenderer.Init(); 
            Xamarin.Essentials.Platform.GetCurrentUIViewController(); 
            Sharpnado.Shades.iOS.iOSShadowsRenderer.Initialize();  
            
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
#if DEBUG
            Firebase.CloudMessaging.Messaging.SharedInstance.SetApnsToken(deviceToken, Firebase.CloudMessaging.ApnsTokenType.Sandbox);  
#else
            Firebase.CloudMessaging.Messaging.SharedInstance.SetApnsToken(deviceToken, Firebase.CloudMessaging.ApnsTokenType.Production);
#endif
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string token)
        {
            messaging.ApnsToken = token;
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public class CustomPushHandler : IPushNotificationHandler
        {
            public void OnError(string error)
            {
            }

            public void OnOpened(NotificationResponse response)
            {
            }

            public void OnReceived(IDictionary<string, object> parameters)
            {
            }
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
