

using PMVOnline.Api.Dtos.Accounts;
using PMVOnline.Api.Dtos.Authorizations;
using Refit;
using System.Threading.Tasks;

namespace PMVOnline.Api
{
    public interface AppApi
    {
        [Post("/connect/token")]
        Task<ApiResponse<TokenDto>> ConnectToken([Body(BodySerializationMethod.UrlEncoded)] ConnectTokenRequestDto request);

        [Get("/api/identity/my-profile")]
        Task<ProfileDto> GetMyProfile();
    }
}