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
    public partial class OfferState
    {

        public class DeleteOfferReducer : ActionHandler<DeleteOfferAction>
        {
            private readonly IHttpService _http;
            public DeleteOfferReducer (IStore store, IHttpService http) : base(store)
            {
                _http = http;
            }

            private OfferState State => Store.GetState<OfferState>();

            public override async Task<Unit> Handle (DeleteOfferAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Delete($"{ApiRoutes.Offer.Delete}/{action.Request.Id}?churchId={action.Request.ChurchId}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
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
