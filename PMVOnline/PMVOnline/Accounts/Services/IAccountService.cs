﻿using PMVOnline.Accounts.Models;
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
        Task<bool> SaveDeviceTokenAsync(string token);
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
            var departments = await Api.GetMyDepartment();
            if (result.Content != null)
            {
                var user = new UserModel
                {
                    Id = result.Content.Id,
                    Email = result.Content.Email,
                    HasPassword = result.Content.HasPassword,
                    IsExternal = result.Content.IsExternal,
                    Name = result.Content.Name,
                    PhoneNumber = result.Content.PhoneNumber,
                    Surname = result.Content.Surname,
                    UserName = result.Content.UserName
                };


                if (departments.Content != null)
                {
                    user.Departments = departments.Content.Select(d => new DepartmentModel { Id = d.DepartmentId, IsLeader = d.IsLeader, Name = d.Department?.Name }).ToArray();
                } 

                applicationSettings.User = user;
                return user;
            }
            return null;
        }

        public async Task<bool> SaveDeviceTokenAsync(string token)
        {
            var result = await Api.SaveDeviceToken(new PMVOnline.Api.Dtos.Tasks.SaveDeviceTokenRequestDto { Token = token });
            return result?.Content == true;
        }
    }
}
