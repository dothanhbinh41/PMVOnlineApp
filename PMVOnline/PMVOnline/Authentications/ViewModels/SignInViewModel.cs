using Plugin.FirebasePushNotification;
using PMVOnline.Accounts.Services;
using PMVOnline.Authentications.Services;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Authentications.ViewModels
{
    public class SignInViewModel : ViewModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

        readonly IAuthenticationSerivce authenticationSerivce;
        readonly IAccountService accountService;
        readonly INavigationService navigationService;
        private readonly IApplicationSettings applicationServices;

        public SignInViewModel(
            IAuthenticationSerivce authenticationSerivce,
            IAccountService accountService,
            INavigationService navigationService,
            IApplicationSettings applicationServices)
        {
            this.authenticationSerivce = authenticationSerivce;
            this.accountService = accountService;
            this.navigationService = navigationService;
            this.applicationServices = applicationServices;
        }
         
        ICommand _SignInCommand;
        public ICommand SignInCommand => _SignInCommand = _SignInCommand ?? new AsyncCommand(ExecuteSignInCommand);
        async Task ExecuteSignInCommand()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return;
            }
            IsBusy = true;
            var result = await authenticationSerivce.SignInAsync(Username, Password);
            if (result)
            {
                var user = await accountService.GetAccountInformationAsync();
                applicationServices.User = user;
                await SendToken();
                await navigationService.NavigateAsync(Routes.Home);
                IsBusy = false;
                Toast("Đăng nhập thành công!");
            }
            else
            {
                IsBusy = false;
                Toast("Đăng nhập thất bại");
            }
        }
         
        ICommand _LostPasswordCommand;
        public ICommand LostPasswordCommand => _LostPasswordCommand = _LostPasswordCommand ?? new AsyncCommand(ExecuteLostPasswordCommand);
        async Task ExecuteLostPasswordCommand()
        {
        }

        async Task SendToken()
        {
            try
            {
                var token = await CrossFirebasePushNotification.Current.GetTokenAsync();
                await accountService.SaveDeviceTokenAsync(token);
            }
            catch (Exception)
            {

            }
        }
    }
}
