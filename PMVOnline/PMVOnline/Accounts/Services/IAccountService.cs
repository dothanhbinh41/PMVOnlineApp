using PMVOnline.Accounts.Models;
using PMVOnline.Api;
using PMVOnline.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (result.Content != null)
            {
                var user = new UserModel
                {
                    Email = result.Content.Email,
                    HasPassword = result.Content.HasPassword,
                    IsExternal = result.Content.IsExternal,
                    Name = result.Content.Name,
                    PhoneNumber = result.Content.PhoneNumber,
                    Surname = result.Content.Surname,
                    UserName = result.Content.UserName,
                    Roles = result.Content?.Roles?.Select(d => new RoleModel { Id = d.Id, Name = d.Name })?.ToArray()
                };
                applicationSettings.User = user;
                return user;
            }
            return null;
        }
    }
}
