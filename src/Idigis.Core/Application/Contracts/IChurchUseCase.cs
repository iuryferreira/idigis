using System.Threading.Tasks;
using Idigis.Core.Dtos.Requests;
using Idigis.Core.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface IChurchUseCase
    {
        AbstractNotificator Notificator { get; }

        public Task<CreateChurchResponse> Add (CreateChurchRequest data);
    }
}
