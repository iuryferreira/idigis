using System;
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

        public class AddOfferReducer : ActionHandler<AddOfferAction>
        {
            private readonly IHttpService _http;
            public AddOfferReducer (IStore store, IHttpService http) : base(store)
            {
                _http = http;
            }

            private OfferState State => Store.GetState<OfferState>();

            public override async Task<Unit> Handle (AddOfferAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Post(ApiRoutes.Offer.Add, action.Request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
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
