using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using External.Client.Helpers;
using External.Client.Models;
using External.Client.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace External.Client.Services
{
    public interface IAccountService
    {
        public List<Error> Errors { get; }
        Task<bool> Signup (CreateChurchRequest model);
        Task<bool> Login (LoginRequest model);
    }

    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AccountService (HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
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
                        await _localStorageService.SetItemAsync("email_auto_fill", CustomEncoder.Encode(church?.Email));
                        return true;
                    }
                case HttpStatusCode.BadRequest:
                    Errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                    return false;
                default:
                    Errors.Add(new() { Key = "Server", Message = "Ops! Ocorreu um erro. Contate o suporte" });
                    return false;
            }
        }

        public async Task<bool> Login (LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ServerRoutes.Church.Signin, model);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    {
                        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
                        var church = new Church { Id = login?.Id, Name = login?.Name, Email = login?.Email };
                        await _localStorageService.SetItemAsync("church", CustomEncoder.Encode(church));
                        await _localStorageService.SetItemAsync("token", CustomEncoder.Encode(login?.Token));
                        ((AuthStateProvider)_authenticationStateProvider).DefineAsAuthenticated(church.Email);
                        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", login?.Token);
                        return true;
                    }
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                    Errors = await response.Content.ReadFromJsonAsync<List<Error>>();
                    return false;
                default:
                    Errors.Add(new() { Key = "Server", Message = "Ops! Ocorreu um erro. Contate o suporte" });
                    return false;
            }
        }
    }
}
