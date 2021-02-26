using Acr.UserDialogs;
using PMVOnline.Api;
using PMVOnline.AppResources;
using PMVOnline.Localization;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PMVOnline.Authentications.Views;
using PMVOnline.Authentications.ViewModels;
using PMVOnline.Homes.Views;
using PMVOnline.Homes.ViewModels;
using PMVOnline.Tasks.Views;
using PMVOnline.Guides.Views;
using PMVOnline.Accounts.Views;
using PMVOnline.Tasks.ViewModels;
using PMVOnline.Common.Services;
using PMVOnline.Guides.ViewModels;
using PMVOnline.Accounts.ViewModels;

namespace PMVOnline
{
    public partial class App : PrismApplication, IResourceManagerProvider
    {
        const string ResourceId = "NewMyDoctor.Localization.MyDoctorResources";
        static ResourceManager _resourceManager;
        public ResourceManager ResourceManager => _resourceManager = _resourceManager ?? new ResourceManager(ResourceId, typeof(App).GetTypeInfo().Assembly);

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {
            //var culture = new CultureInfo("en-US");

            ////Culture for any thread
            //CultureInfo.DefaultThreadCurrentCulture = culture;

            ////Culture for UI in any thread
            //CultureInfo.DefaultThreadCurrentUICulture = culture;
            ////  AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            //// StringResources 
            EdutalkResource.Culture = CultureInfo.InstalledUICulture;

        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            VersionTracking.Track();
            var result = await NavigationService.NavigateAsync(Routes.Home);
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=ff9085a0-5b3d-427f-8003-4005cfef9339;ios=7a5ae654-4193-4e5e-9525-663f5ededef0", typeof(Analytics), typeof(Crashes));

            Initialize();
        }

        async Task Initialize()
        {
            var token = await CrossFirebasePushNotification.Current.GetTokenAsync();

            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {

            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
            };
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                MessagingCenter.Send("1", "OnResume");

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<PMVOnlineApiModule>();
            moduleCatalog.AddModule<PMVOnlineLocalizationModule>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterExternalService(containerRegistry);
            RegisterForNavigation(containerRegistry);
            RegisterService(containerRegistry);
            RegisterDialogs(containerRegistry);
        }

        void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomeViewModel>();
            containerRegistry.RegisterForNavigation<TaskPage, TaskViewModel>();
            containerRegistry.RegisterForNavigation<GuidePage, GuideViewModel>();
            containerRegistry.RegisterForNavigation<AccountPage, AccountViewModel>();
            containerRegistry.RegisterForNavigation<CreateTaskPage, CreateTaskViewModel>();
        }
        void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterDialog<AddressUnitDialog, AddressUnitDialogViewModel>(); 
        }

        void RegisterService(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IResourceManagerProvider>(this);
            containerRegistry.Register<IApplicationSettings, ApplicationSettings>();
        }
        void RegisterExternalService(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            //containerRegistry.RegisterInstance(Plugin.Permissions.CrossPermissions.Current);
            //containerRegistry.RegisterInstance(PopupNavigation.Instance);
            // containerRegistry.RegisterPopupNavigationService();
        }
    }

    public sealed partial class Routes
    {
        static readonly string navigation = nameof(NavigationPage);
        public static readonly Uri SignIn = new Uri($"{nameof(SignInPage)}", UriKind.Relative);
        public static readonly Uri Home = new Uri($"{nameof(MainPage)}?SelectedTab={nameof(HomePage)}", UriKind.Relative);

        public static readonly Uri CreateTask = new Uri($"{nameof(CreateTaskPage)}", UriKind.Relative);
    }
    public sealed partial class DialogRoutes
    {
        //public static readonly string AddressUnit = $"{nameof(AddressUnitDialog)}"; 
    }

    public class NavigationKey
    {
        public const string ExistUser = "_MyDoctor_ExistUser_Key_";
    }
}
