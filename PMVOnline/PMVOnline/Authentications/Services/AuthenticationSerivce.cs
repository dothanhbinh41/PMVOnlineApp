﻿using PMVOnline.Api;
using PMVOnline.Api.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Authentications.Services
{
    public interface IAuthenticationSerivce
    {
        Task<bool> SignInAsync(string userName, string password);
    }

    public class AuthenticationSerivce : ApiProvider<AppApi>, IAuthenticationSerivce
    {
        readonly IAuthHeaderManager authHeader;

        public AuthenticationSerivce(IAuthHeaderManager authHeader)
        {
            this.authHeader = authHeader;
        }

        public async Task<bool> SignInAsync(string userName, string password)
        {
            var result = await Api.ConnectToken(new PMVOnline.Api.Dtos.Authorizations.ConnectTokenRequestDto { Username = userName, Password = password });
            if (string.IsNullOrEmpty(result?.AccessToken))
            {
                return false;
            }

            return await authHeader.SetAuthHeaderAsync<object>(new Token { AccessToken = result.AccessToken, RefreshToken = result.RefreshToken });
        }
    }
}
