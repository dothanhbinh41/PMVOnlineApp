

using PMVOnline.Api.Dtos.Authorizations;
using Refit;
using System.Threading.Tasks;

namespace PMVOnline.Api
{
    public interface AppApi
    {
        [Get("/connect/token")]
        Task<TokenDto> ConnectToken([Body(BodySerializationMethod.UrlEncoded)] ConnectTokenRequestDto request);
    }
}