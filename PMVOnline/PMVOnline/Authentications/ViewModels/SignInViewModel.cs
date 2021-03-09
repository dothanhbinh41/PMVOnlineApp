using PMVOnline.Authentications.Services;
using PMVOnline.Common.Bases;
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
        public SignInViewModel(IAuthenticationSerivce authenticationSerivce)
        {
            this.authenticationSerivce = authenticationSerivce;
        }



        ICommand _SignInCommand;
        public ICommand SignInCommand => _SignInCommand = _SignInCommand ?? new AsyncCommand(ExecuteSignInCommand);
        async Task ExecuteSignInCommand()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return;
            }
            var result = await authenticationSerivce.SignInAsync(Username, Password);
            if (result)
            {

            }
            else
            {

            }
        }



        ICommand _LostPasswordCommand;
        public ICommand LostPasswordCommand => _LostPasswordCommand = _LostPasswordCommand ?? new AsyncCommand(ExecuteLostPasswordCommand);
        async Task ExecuteLostPasswordCommand()
        {
        }
    }
}
