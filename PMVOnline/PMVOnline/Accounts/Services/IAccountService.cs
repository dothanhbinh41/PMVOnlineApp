using PMVOnline.Accounts.Models;
using PMVOnline.Api;
using PMVOnline.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Accounts.Services
{
    public interface IAccountService
    {
        Task<UserModel> GetAccountInformationAsync();
    }

    public class AccountService : AuthApiProvider<AppApi>, IAccountService
    {

        readonly IApplicationSettings applicationSettings;

        public AccountService(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<UserModel> GetAccountInformationAsync()
        {
            var result = await Api.GetMyProfile();
            if (result != null)
            {
                var user = new UserModel
                {
                    Email = result.Email,
                    HasPassword = result.HasPassword,
                    IsExternal = result.IsExternal,
                    Name = result.Name,
                    PhoneNumber = result.PhoneNumber,
                    Surname = result.Surname,
                    UserName = result.UserName
                };
                applicationSettings.User = user;
                return user;
            }
            return null;
        }
    }
}
