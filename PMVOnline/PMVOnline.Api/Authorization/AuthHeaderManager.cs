using System;
using System.Threading.Tasks; 
using PMVOnline.Api.Extensions;
using Xamarin.Essentials;

namespace PMVOnline.Api.Authorization
{
    public class AuthHeaderManager : IAuthHeaderManager
    {
        public static string AuthHeaderNewKey = "_Edutalk_AuthHeaderKey_";
        public Task<T> GetAuthHeaderAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Task.FromResult(default(T));
            }
            return Task.FromResult((Preferences.Get(key, string.Empty)).DeserializeObject<T>());
        }

        public Task<T> GetAuthHeaderAsync<T>()
        {
            return GetAuthHeaderAsync<T>(AuthHeaderNewKey);
        }


        public Task<bool> SetAuthHeaderAsync<T>(string key, T header)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Task.FromResult(false);
            }
            if (header == null)
            {
                Preferences.Remove(key);
                return Task.FromResult(true);
            }
            Preferences.Set(key, header.SerializeObject());
            return Task.FromResult(true);
        }

        public Task<bool> SetAuthHeaderAsync<T>(T header)
        {
            return SetAuthHeaderAsync(AuthHeaderNewKey, header);
        }
    }
}
