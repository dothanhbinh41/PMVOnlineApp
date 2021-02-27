using PMVOnline.Accounts.Models;
using PMVOnline.Accounts.Services;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Accounts.ViewModels
{
    public class AccountViewModel : TabViewModelBase
    {
        public string Version => VersionTracking.CurrentVersion;
        public bool UserNotification { get; set; } = true;
        public UserModel User { get; set; }

        readonly INavigationService navigationService;
        readonly IApplicationSettings applicationSettings;
        readonly IOpenSettingService openSettingService;

        public AccountViewModel(
            INavigationService navigationService, 
            IApplicationSettings applicationSettings,
            IOpenSettingService openSettingService)
        {
            this.navigationService = navigationService;
            this.applicationSettings = applicationSettings;
            this.openSettingService = openSettingService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            User = applicationSettings.User;
            UserNotification = applicationSettings.UseNotification;
        }

        ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand = _logoutCommand ?? new AsyncCommand(ExecuteLogoutCommand);
        async Task ExecuteLogoutCommand()
        {
            await navigationService.NavigateAsync(Routes.SignIn);
        }


        ICommand _ChangePasswordCommand;
        public ICommand ChangePasswordCommand => _ChangePasswordCommand = _ChangePasswordCommand ?? new AsyncCommand(ExecuteChangePasswordCommand);
        async Task ExecuteChangePasswordCommand()
        {
        }

        ICommand _ChangeFontsizeCommand;
        public ICommand ChangeFontsizeCommand => _ChangeFontsizeCommand = _ChangeFontsizeCommand ?? new Command(ExecuteChangeFontsizeCommand);
        void ExecuteChangeFontsizeCommand()
        {
            openSettingService.OpenSetting();

            Application.Current.Resources["FontSizeSmall"] = 14;
            Application.Current.Resources["FontSizeNormal"] = 17;
            Application.Current.Resources["FontSizeTitle"] = 20;
        }

        public void OnUserNotificationChanged()
        {
            applicationSettings.UseNotification = UserNotification;
        }
    }
}
