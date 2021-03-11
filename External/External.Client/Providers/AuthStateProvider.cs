using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using External.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace External.Client.Providers
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public AuthStateProvider (HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync ()
        {
            var encoded = await _localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrWhiteSpace(encoded))
            {
                return new(new(new ClaimsIdentity()));
            }

            var token = CustomEncoder.Decode<string>(encoded);
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
            return new(new(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        public void DefineAsAuthenticated (string email)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "jwt"));
            var stateTask = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(stateTask);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt (string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs is null
                ? new List<Claim>()
                : keyValuePairs.Select(kvp =>
                    new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));

            static byte[] ParseBase64WithoutPadding (string base64)
            {
                switch (base64.Length % 4)
                {
                    case 2:
                        base64 += "==";
                        break;
                    case 3:
                        base64 += "=";
                        break;
                }

                return Convert.FromBase64String(base64);
            }
        }
    }
}
