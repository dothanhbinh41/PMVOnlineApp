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
using PMVOnline.Tasks.Dialogs;
using PMVOnline.Tasks.Views.Admins;
using PMVOnline.Tasks.ViewModels.Admins;
using PMVOnline.Common.Controls;
using PMVOnline.Accounts.Services;
using DryIoc;
using PMVOnline.Tasks.Services;
using PMVOnline.Authentications.Services;
using PMVOnline.Guides.Services;
using PMVOnline.Tasks.ViewModels.Dialogs;

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
            await NavigationService.NavigateAsync(Routes.Welcome);
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=840c1b8d-55b3-4ac2-b946-e54277ee2a58;ios=7a5ae654-4193-4e5e-9525-663f5ededef0", typeof(Analytics), typeof(Crashes));
            Container.Resolve<IFontsizeService>().Init();
            Initialize();
        }

        async Task Initialize()
        {
            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {

            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                if (MainPage is NavigationPage nav && nav.CurrentPage is TabbedPage tab)
                {
                    for (int i = 0; i < tab.Children.Count; i++)
                    {
                        if (tab.Children[i].BindingContext is HomeViewModel h)
                        {
                            h.ReloadCommand?.Execute(null);
                        }

                        if (tab.Children[i].BindingContext is TaskViewModel t)
                        {
                            t.ReloadCommand?.Execute(null);
                        }
                    }
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //if (MainPage is NavigationPage nav && nav.CurrentPage is TabbedPage tab)
                //{
                //    for (int i = 0; i < tab.Children.Count; i++)
                //    {
                //        if (tab.Children[i].BindingContext is HomeViewModel h)
                //        {
                //            h.ReloadCommand?.Execute(null);
                //        }

                //        if (tab.Children[i].BindingContext is TaskViewModel t)
                //        {
                //            t.ReloadCommand?.Execute(null);
                //        }
                //    }
                //}
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
            containerRegistry.RegisterForNavigation<TaskReferencePage, TaskReferenceViewModel>();
            containerRegistry.RegisterForNavigation<ModerateTaskPage, ModerateTaskViewModel>();
            containerRegistry.RegisterForNavigation<TaskDetailPage, Tasks.ViewModels.TaskDetailViewModel>();
            containerRegistry.RegisterForNavigation<CommentPage, CommentViewModel>();
            containerRegistry.RegisterForNavigation<HistoryPage, HistoryViewModel>();
            containerRegistry.RegisterForNavigation<WelcomePage, WelcomeViewModel>();
            containerRegistry.RegisterForNavigation<CompleteTaskPage, CompleteTaskViewModel>();
        }
        void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ChooseTargetDialog, ChooseTargetViewModel>();
            containerRegistry.RegisterDialog<ChoosePriorityDialog, ChoosePriorityViewModel>();
            containerRegistry.RegisterDialog<MultiSelectTaskDialog, MultiSelectTaskViewModel>();
            containerRegistry.RegisterDialog<SelectTaskDialog, SelectTaskViewModel>();
            containerRegistry.RegisterDialog<ChooseUsersDialog, ChooseUsersViewModel>();
            containerRegistry.RegisterDialog<WriteNoteDialog, WriteNoteViewModel>();
            containerRegistry.RegisterDialog<TaskDetailDialog, Tasks.ViewModels.Dialogs.TaskDetailViewModel>();
        }

        void RegisterService(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IResourceManagerProvider>(this);
            containerRegistry.Register<IApplicationSettings, ApplicationSettings>();
            containerRegistry.Register<IFontsizeService, FontsizeService>();
            containerRegistry.Register<ITaskService, TaskService>();
            containerRegistry.Register<IAuthenticationSerivce, AuthenticationSerivce>();
            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IFileService, FileService>();
            containerRegistry.Register<IGuideService, GuideService>();
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
        //public static readonly Uri SignIn = new Uri($"{nameof(SignInPage)}", UriKind.Relative);
        //public static readonly Uri Home = new Uri($"{nameof(MainPage)}?SelectedTab={nameof(HomePage)}", UriKind.Relative);
        public static readonly Uri Home = new Uri($"PMVOnline:///{navigation}/{nameof(MainPage)}", UriKind.Absolute);
        public static readonly Uri SignIn = new Uri($"PMVOnline:///{navigation}/{nameof(SignInPage)}", UriKind.Absolute);

        public static readonly Uri CreateTask = new Uri($"{nameof(CreateTaskPage)}", UriKind.Relative);
        public static readonly Uri TaskReference = new Uri($"{nameof(TaskReferencePage)}", UriKind.Relative);
        public static readonly Uri ModerateTask = new Uri($"{nameof(ModerateTaskPage)}", UriKind.Relative);
        public static readonly Uri TaskDetail = new Uri($"{nameof(TaskDetailPage)}", UriKind.Relative);
        public static readonly Uri Comment = new Uri($"{nameof(CommentPage)}", UriKind.Relative);
        public static readonly Uri History = new Uri($"{nameof(HistoryPage)}", UriKind.Relative);
        public static readonly Uri Welcome = new Uri($"{nameof(WelcomePage)}", UriKind.Relative);
        public static readonly Uri CompleteTask = new Uri($"{nameof(CompleteTaskPage)}", UriKind.Relative);
    }
    public sealed partial class DialogRoutes
    {
        public static readonly string ChooseTarget = $"{nameof(ChooseTargetDialog)}";
        public static readonly string ChoosePriority = $"{nameof(ChoosePriorityDialog)}";
        public static readonly string MultiSelectTask = $"{nameof(MultiSelectTaskDialog)}";
        public static readonly string SelectTask = $"{nameof(SelectTaskDialog)}";
        public static readonly string ChooseUsers = $"{nameof(ChooseUsersDialog)}";
        public static readonly string WriteNote = $"{nameof(WriteNoteDialog)}";
        public static readonly string TaskDetail = $"{nameof(TaskDetailDialog)}";
    }

    public class NavigationKey
    {
        public const string Priority = "Priority";
        public const string Target = "Target";
        public const string ReferenceTasks = "ReferenceTasks";
        public const string CloneTask = "CloneTask";
        public const string MyTasks = "MyTasks";
        public const string TaskId = "TaskId";
        public const string Reload = "Reload";
        public const string Note = "Note";
        public const string Users = "Users";
        public const string SelectedUsers = "SelectedUsers";
        public const string Completed = "Completed";
        public const string Editable = "Editable";
        public const string Count = "Count";
        public const string AllTargets = "AllTargets";
    }
}
