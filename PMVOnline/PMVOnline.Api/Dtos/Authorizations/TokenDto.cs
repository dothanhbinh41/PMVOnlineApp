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
        public string GrantType => ApiBase.PasswordGrantType;
        public string ClientId => ApiBase.ClientId;
        public string ClientSecret => ApiBase.ClientSecret;
        public string Scope => ApiBase.ClientScope;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
