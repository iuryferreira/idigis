using System.Threading.Tasks;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface IChurchUseCase
    {
        AbstractNotificator Notificator { get; }
        public Task<CreateChurchResponse> Add (CreateChurchRequest data);
        public Task<GetChurchResponse> Get (GetChurchRequest data);
        public Task<EditChurchResponse> Edit (EditChurchRequest data);
        public Task<DeleteChurchResponse> Delete (DeleteChurchRequest data);
    }
}
