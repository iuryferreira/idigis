using System.Threading;
using System.Threading.Tasks;
using Core.Application.Contracts;
using Core.Application.Requests;
using Core.Application.Responses;

namespace Core.Application.Handlers
{
    public class LoginHandler : ILoginHandler
    {
        public Task<LoginResponse> Handle (LoginRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
