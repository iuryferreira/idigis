using System.Threading.Tasks;
using Idigis.Core.Domain.Dtos.Requests;
using Idigis.Core.Domain.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application
{
    public interface IChurchUseCase
    {
        AbstractNotificator Notificator { get; }

        public Task<CreateChurchResponse> Add (CreateChurchRequest data);
    }
}
