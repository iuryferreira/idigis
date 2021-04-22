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
    public partial class MemberState
    {
        public class LoadMembersReducer : ActionHandler<LoadMembersAction>
        {
            private readonly IHttpService _http;
            private readonly ILocalStorageService _storage;

            public LoadMembersReducer (IStore store, IHttpService http, ILocalStorageService storage) : base(store)
            {
                _storage = storage;
                _http = http;
            }

            private MemberState State => Store.GetState<MemberState>();
            private ChurchState ChurchState => Store.GetState<ChurchState>();

            public override async Task<Unit> Handle (LoadMembersAction action, CancellationToken cancellationToken)
            {
                var response = await _http.Get($"{ApiRoutes.Member.List}?churchId={ChurchState.Church.Id}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result =
                            await response.Content.ReadFromJsonAsync<List<GetMemberResponse>>(
                                cancellationToken: cancellationToken);
                        var members = result?.Select(m => new Member
                        {
                            Id = m.Id,
                            FullName = m.FullName,
                            BaptismDate = m.BaptismDate,
                            BirthDate = m.BirthDate,
                            PhoneNumber = m.Contact.PhoneNumber,
                            HouseNumber = m.Contact.HouseNumber,
                            Street = m.Contact.Street,
                            District = m.Contact.District,
                            City = m.Contact.City
                        }).ToList() ?? new();
                        State.Members = members;
                        State.TotalMembers = State.Members.Count;
                        await _storage.SetItemAsync("members", CustomEncoder.Encode(State.Members));
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
