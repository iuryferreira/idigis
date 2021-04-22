using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazorState;
using Idigis.Web.Models;
using Idigis.Web.Services;
using MediatR;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class EditChurchReducer : ActionHandler<EditChurchAction>
        {
            private readonly IHttpService _http;

            public EditChurchReducer (IStore store, IHttpService http) : base(store)
            {
                _http = http;
            }

            private ChurchState State => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (EditChurchAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Put($"{ApiRoutes.Church.Edit}/{action.Request.Id}", action.Request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
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
