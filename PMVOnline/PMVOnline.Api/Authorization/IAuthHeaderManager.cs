using System;
using System.Threading.Tasks;

namespace PMVOnline.Api.Authorization
{
    public interface IAuthHeaderManager
    { 
        Task<T> GetAuthHeaderAsync<T>(string key);
        Task<T> GetAuthHeaderAsync<T>();
        Task<bool> SetAuthHeaderAsync<T>(string key, T header);
        Task<bool> SetAuthHeaderAsync<T>(T header);
    }
}