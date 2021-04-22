using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Idigis.Web.Helpers;
using Idigis.Web.Models;
using Idigis.Web.Services;
using MediatR;

namespace Idigis.Web.States
{
    public partial class ChurchState
    {
        public class LoadStateReducer : ActionHandler<LoadStateAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public LoadStateReducer (IStore store, ILocalStorageService storage, IHttpService http) : base(store)
            {
                _storage = storage;
                _http = http;
            }

            private ChurchState State => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (LoadStateAction action, CancellationToken cancellationToken)
            {
                var church = CustomEncoder.Decode<Church>(await _storage.GetItemAsStringAsync("church"));
                var response = await _http.Post(ApiRoutes.Church.Refresh, new LoginRequest { Email = church.Email });
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var content =
                            await response.Content.ReadFromJsonAsync<LoginResponse>(
                                cancellationToken: cancellationToken);
                        State.Church = new() { Id = content?.Id, Email = content?.Email, Name = content?.Name };
                        State.Token = content?.Token;
                        State.Authenticated = true;
                        await _storage.SetItemAsync("church", CustomEncoder.Encode(church));
                        await _storage.SetItemAsync("token", CustomEncoder.Encode(content?.Token));
                        break;
                    case HttpStatusCode.Unauthorized:
                        State.Authenticated = false;
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
