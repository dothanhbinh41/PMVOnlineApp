using PMVOnline.Authentications.Services;
using PMVOnline.Common.Bases;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Homes.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IAuthenticationSerivce authenticationService;

        public WelcomeViewModel(INavigationService navigationService, IAuthenticationSerivce authenticationService)
        {
            this.navigationService = navigationService;
            this.authenticationService = authenticationService;
        }

        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            CheckLogin();
        }

        async Task CheckLogin()
        {
            var loged = await authenticationService.IsAuthenticated();
            if (loged)
            {
                await navigationService.NavigateAsync(Routes.Home);
            }
            else
            {
                await navigationService.NavigateAsync(Routes.SignIn);
            }
        }
    }
}
