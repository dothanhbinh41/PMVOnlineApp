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
        public Fontsize Fontsize { get; set; }
        readonly INavigationService navigationService;
        readonly IApplicationSettings applicationSettings; 
        private readonly IFontsizeService fontsizeService;

        public AccountViewModel(
            INavigationService navigationService, 
            IApplicationSettings applicationSettings, 
            IFontsizeService fontsizeService)
        {
            this.navigationService = navigationService;
            this.applicationSettings = applicationSettings; 
            this.fontsizeService = fontsizeService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            User = applicationSettings.User;
            UserNotification = applicationSettings.UseNotification;
            Fontsize = fontsizeService.CurrentSize;
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
            switch (Fontsize)
            { 
                case Fontsize.Large: 
                    fontsizeService.ChangeFontsize(Fontsize.Normal);
                    break;
                case Fontsize.Normal:
                default: 
                    fontsizeService.ChangeFontsize(Fontsize.Large);
                    break;
            } 
            Fontsize = fontsizeService.CurrentSize;
        }

        public void OnUserNotificationChanged()
        {
            applicationSettings.UseNotification = UserNotification;
        }
    }
}
