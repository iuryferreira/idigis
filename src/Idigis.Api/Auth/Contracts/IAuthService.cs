using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;

namespace Idigis.Api.Auth.Contracts
{
    public interface IAuthService
    {
        LoginResponse Authenticate (LoginRequest request);
        public LoginResponse RefreshToken (LoginRequest request);
    }
}
