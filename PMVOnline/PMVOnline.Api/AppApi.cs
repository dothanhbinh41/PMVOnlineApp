

using PMVOnline.Api.Dtos.Accounts;
using PMVOnline.Api.Dtos.Authorizations;
using PMVOnline.Api.Dtos.Tasks;
using Refit;
using System.Threading.Tasks;

namespace PMVOnline.Api
{
    public interface AppApi
    {
        [Post("/connect/token")]
        Task<ApiResponse<TokenDto>> ConnectToken([Body(BodySerializationMethod.UrlEncoded)] ConnectTokenRequestDto request);

        [Get("/api/identity/my-profile")]
        Task<ApiResponse<ProfileDto>> GetMyProfile();
         
        [Get("/api/app/task/my-tasks?MaxResultCount={max}&SkipCount={skip}")]
        Task<ApiResponse<TaskDto[]>> GetMyTasks(int skip, int max);

        [Get("/api/app/task/my-actions")]
        Task<ApiResponse<TaskDto[]>> GetMyActions();
    }
}