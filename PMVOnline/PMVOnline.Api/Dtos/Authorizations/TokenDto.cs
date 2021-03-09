using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Api.Dtos.Authorizations
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
    }

    public class ConnectTokenRequestDto
    {
        [AliasAs("grant_type")]
        public string GrantType => ApiBase.PasswordGrantType;

        [AliasAs("client_id")]
        public string ClientId => ApiBase.ClientId;

        [AliasAs("client_secret")]
        public string ClientSecret => ApiBase.ClientSecret;

        [AliasAs("scope")]
        public string Scope => ApiBase.ClientScope;

        [AliasAs("username")]
        public string Username { get; set; }

        [AliasAs("password")]
        public string Password { get; set; }
    }
}
