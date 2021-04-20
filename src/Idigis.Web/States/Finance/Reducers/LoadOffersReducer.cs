using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Shared.Dtos.Responses;
using Idigis.Web.Helpers;
using Idigis.Web.Models;
using Idigis.Web.Services;
using MediatR;

namespace Idigis.Web.States
{
    public partial class FinanceState
    {
        public class LoadOffersReducer : ActionHandler<LoadOffersAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public LoadOffersReducer (IStore store, IHttpService http, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
                _http = http;
            }

            private FinanceState State => Store.GetState<FinanceState>();
            private ChurchState ChurchState => Store.GetState<ChurchState>();


            public override async Task<Unit> Handle (LoadOffersAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Get($"{ApiRoutes.Offer.List}?churchId={ChurchState.Church.Id}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result =
                            await response.Content.ReadFromJsonAsync<List<GetOfferResponse>>(
                                cancellationToken: cancellationToken);
                        var offers = result?.Select(o => new Offer { Id = o.Id, Value = o.Value }).ToList();
                        State.Offers = offers;
                        State.TotalOffersInMonth = (State.Offers ?? new()).Sum(o => o.Value);
                        await _storage.SetItemAsync("offers", CustomEncoder.Encode(State.Offers));
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
