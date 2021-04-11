using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface ITitheUseCase
    {
        AbstractNotificator Notificator { get; }
        public Task<CreateTitheResponse> Add (CreateTitheRequest data);
        public Task<GetTitheResponse> Get (GetTitheRequest data);
        public Task<List<GetTitheResponse>> List (ListTitheRequest data);
        public Task<EditTitheResponse> Edit (EditTitheRequest data);
        public Task<DeleteTitheResponse> Delete (DeleteTitheRequest data);
    }
}
