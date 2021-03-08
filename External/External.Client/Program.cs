using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using External.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace External.Client
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);
            builder.Services.AddScoped(_ => new HttpClient {BaseAddress = new("https://localhost:5001")});
            await builder.Build().RunAsync();
        }
    }
}
