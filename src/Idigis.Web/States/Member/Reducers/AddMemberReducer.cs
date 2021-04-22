using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Web.Models;
using Idigis.Web.Services;
using MediatR;

namespace Idigis.Web.States
{
    public partial class MemberState
    {
        public class AddMemberReducer : ActionHandler<AddMemberAction>
        {
            private readonly IHttpService _http;

            public AddMemberReducer (IStore store, IHttpService http) : base(store)
            {
                _http = http;
            }

            private MemberState State => Store.GetState<MemberState>();

            public override async Task<Unit> Handle (AddMemberAction action, CancellationToken cancellationToken)
            {
                var data = new CreateMemberRequest
                {
                    ChurchId = action.Request.ChurchId,
                    FullName = action.Request.FullName,
                    BaptismDate = action.Request.BaptismDate,
                    BirthDate = action.Request.BirthDate,
                    Contact = action.Request.Contact
                };
                var contactIsNotNull = data.Contact.GetType()
                    .GetProperties()
                    .Select(pi => pi.GetValue(data.Contact))
                    .Any(value => value != null);
                if (!contactIsNotNull)
                {
                    data.Contact = null;
                }
                var response = await _http.Post(ApiRoutes.Member.Add, data);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        State.Errors =
                            await response.Content.ReadFromJsonAsync<List<Error>>(cancellationToken: cancellationToken);
                        break;
                    default:
                        var errors =
                            await response.Content.ReadFromJsonAsync<List<Error>>(cancellationToken: cancellationToken);
                        var error = errors?.First();
                        State.Errors.Add(new() { Key = "AnyError", Message = error?.Message });
                        break;
                }
                return await Unit.Task;
            }
        }
    }
}
