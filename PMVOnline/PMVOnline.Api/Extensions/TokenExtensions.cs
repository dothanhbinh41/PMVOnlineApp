using System;
using System.Threading.Tasks; 
using PMVOnline.Api.Authorization;

namespace PMVOnline.Api.Extensions
{
    public static class TokenExtensions
    {
        public static async Task<string> GetBearerToken(this IAuthHeaderManager authHeader)
        {
            var token = await authHeader.GetAuthHeaderAsync<Token>();
            return token?.AccessToken;
        }
    }
}
