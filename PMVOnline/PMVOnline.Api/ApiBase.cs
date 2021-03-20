using System;
namespace PMVOnline.Api
{ 
    public class ApiBase
    {
        public const string BearerScheme = "Bearer"; 
        public const string ServerApi = "https://pmvonline.azurewebsites.net"; 
        public const string ClientId = "PMVOnline_App"; 
        public const string ClientSecret = "1q2w3e*"; 
        public const string ClientScope = "offline_access PMVOnline"; 
        public const string PasswordGrantType = "password"; 
    } 
}