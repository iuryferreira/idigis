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
        public Task<GetChurchResponse> Get (GetChurchRequest data);
        public Task<EditChurchResponse> Edit (EditChurchRequest data);
        public Task<DeleteChurchResponse> Delete (DeleteChurchRequest data);
    }
}
