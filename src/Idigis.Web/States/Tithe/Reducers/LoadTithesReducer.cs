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
    public partial class TitheState
    {
        public class LoadTithesReducer : ActionHandler<LoadTithesAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public LoadTithesReducer (IStore store, IHttpService http, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
                _http = http;
            }

            private TitheState State => Store.GetState<TitheState>();
            private ChurchState ChurchState => Store.GetState<ChurchState>();


            public override async Task<Unit> Handle (LoadTithesAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Get($"{ApiRoutes.Tithe.List}?churchId={ChurchState.Church.Id}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result =
                            await response.Content.ReadFromJsonAsync<List<GetTitheResponse>>(
                                cancellationToken: cancellationToken);
                        var tithes = result?.Select(t =>
                                new Tithe { Id = t.Id, Value = t.Value, Date = t.Date, MemberName = t.MemberName, MemberId = t.MemberId})
                            .ToList();
                        State.Tithes = tithes;
                        State.TotalTithesInMonth = (State.Tithes ?? new()).Sum(o => o.Value);
                        await _storage.SetItemAsync("offers", CustomEncoder.Encode(State.Tithes));
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
