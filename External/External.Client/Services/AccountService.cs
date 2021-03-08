using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using External.Client.Helpers;
using External.Client.Models;

namespace External.Client.Services
{
    public interface IAccountService
    {
        public List<Error> Errors { get; }
        Task<bool> Signup (CreateChurchRequest model);
    }

    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public AccountService (HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public List<Error> Errors { get; private set; } = new();

        public async Task<bool> Signup (CreateChurchRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ServerRoutes.Church.Signup, model);
            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                {
                    var church = await response.Content.ReadFromJsonAsync<CreateChurchResponse>();
                    await _localStorageService.SetItemAsync("email_auto_fill", church?.Email);
                    return true;
                }
                case HttpStatusCode.BadRequest:
                    Errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                    return false;
                default:
                    Errors.Add(new() {Key = "Server", Message = "Ops! Ocorreu um erro. Contate o suporte"});
                    return false;
            }
        }
    }
}
