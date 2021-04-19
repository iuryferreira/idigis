using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Idigis.Web.Helpers;
using Idigis.Web.Models;

namespace Idigis.Web.Services
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> Post<T> (string uri, T data);
        Task<HttpResponseMessage> Get (string uri);
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;

        public HttpService (HttpClient http, ILocalStorageService localStorageService)
        {
            _http = http;
            _localStorageService = localStorageService;
        }

        public async Task<HttpResponseMessage> Get (string uri)
        {
            await HasToken();
            return await _http.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> Post<T> (string uri, T data)
        {
            await HasToken();
            return await _http.PostAsJsonAsync(uri, data);
        }

        private async Task HasToken ()
        {
            var church = await _localStorageService.GetItemAsync<Church>("church");
            if (church is not null && !string.IsNullOrWhiteSpace(church.Token))
            {
                var decoded = CustomEncoder.Decode<string>(church.Token);
                _http.DefaultRequestHeaders.Authorization = new("Bearer", decoded);
            }
        }
    }
}
