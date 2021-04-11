using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Notie.Contracts;

namespace Idigis.Core.Application.Contracts
{
    public interface IMemberUseCase
    {
        AbstractNotificator Notificator { get; }
        public Task<CreateMemberResponse> Add (CreateMemberRequest data);
        public Task<GetMemberResponse> Get (GetMemberRequest data);
        public Task<List<GetMemberResponse>> List (ListMemberRequest data);
        public Task<EditMemberResponse> Edit (EditMemberRequest data);
        public Task<DeleteMemberResponse> Delete (DeleteMemberRequest data);
    }
}
