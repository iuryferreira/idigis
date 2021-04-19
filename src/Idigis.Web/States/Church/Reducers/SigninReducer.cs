using System;
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
    public partial class ChurchState
    {
        public class SigninActionReducer : ActionHandler<SigninAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public SigninActionReducer (IStore store, IHttpService http, ILocalStorageService storage) : base(store)
            {
                _http = http;
                _storage = storage;
            }

            private ChurchState State => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (SigninAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Post(ApiRoutes.Church.Signin, action.Request);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
                        var church = new Church() { Name = result?.Name, Email = result?.Email, Id = result?.Id };
                        await _storage.SetItemAsync("church", CustomEncoder.Encode(church));
                        await _storage.SetItemAsync("token", CustomEncoder.Encode(result?.Token));
                        State.Church = church;
                        State.Authenticated = true;
                        State.Token = result?.Token;
                        break;
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                        State.Errors = await response.Content.ReadFromJsonAsync<List<Error>>(cancellationToken: cancellationToken);
                        break;
                    default:
                        var errors = await response.Content.ReadFromJsonAsync<List<Error>>(cancellationToken: cancellationToken);
                        var error = errors?.First();
                        State.Errors.Add(new() { Key = "AnyError", Message = error?.Message });
                        break;
                }
                return await Unit.Task;
            }
        }
    }
}
