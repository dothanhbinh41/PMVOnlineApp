﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism;
using Prism.Ioc;
using Acr.UserDialogs;
using PMVOnline.Common.Services;
using PMVOnline.Droid.Services;
using PMVOnline.Accounts.Services;
using Plugin.FirebasePushNotification;
using Firebase;
using Android.Content;
using Xamarin.Essentials;
using FFImageLoading.Forms.Platform;

namespace PMVOnline.Droid
{
    [Activity(Label = "PMVOnline", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Init(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidPlatform()));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void Init(Bundle savedInstanceState)
        {
            UserDialogs.Init(this);
            Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            FirebaseApp.InitializeApp(this, new FirebaseOptions.Builder()
                .SetApiKey("AIzaSyC4G-IC8c7BB6Ba-JOEZRiXJXiVtIQAAok")
                .SetApplicationId("com.pmvina.task")
                .SetGcmSenderId("557284700442") 
                .SetProjectId("pmvina-470a2")
                .Build());
            CachedImageRenderer.Init(true);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "general";
            }
            FirebasePushNotificationManager.Initialize(this, false);
            FirebasePushNotificationManager.NotificationActivityType = typeof(MainActivity);
            FirebasePushNotificationManager.IconResource = Resource.Drawable.bg_splash;
            FirebasePushNotificationManager.LargeIconResource = Resource.Drawable.bg_splash;
            FirebasePushNotificationManager.ShouldShowWhen = Preferences.Get("UseNotification", true);
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebasePushNotificationManager.ShouldShowWhen = Preferences.Get("UseNotification", true);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public class AndroidPlatform : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.Register<IDateTimeService, DateTimeService>();
            }
        }
    }
}