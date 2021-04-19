using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Idigis.Web.Helpers;
using Idigis.Web.Models;

namespace Idigis.Web.Services
{
    public interface IChurchService
    {
        List<Error> Errors { get; }
        Task<bool> Signin (LoginRequest request);
    }

    public class ChurchService : IChurchService
    {
        private readonly IHttpService _http;
        private readonly ILocalStorageService _storage;

        public ChurchService (IHttpService http, ILocalStorageService storage)
        {
            _http = http;
            _storage = storage;
            Errors = new();
        }

        public List<Error> Errors { get; set; }

        public async Task<bool> Signin (LoginRequest request)
        {
            var response = await _http.Post(ApiRoutes.Church.Signin, request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    var church = new Church() { Name = result.Name, Email = result.Email, Id = result.Id, Token = result.Token };
                    await _storage.SetItemAsync("church", CustomEncoder.Encode(church));
                    return true;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                    Errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                    return false;
                default:
                    var error = (await response.Content.ReadFromJsonAsync<List<Error>>()).First();
                    Errors.Add(new() { Key = "AnyError", Message = error.Message });
                    return false;
            }
        }

    }
}
