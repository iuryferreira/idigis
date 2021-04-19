using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorState;
using Idigis.Web.Services;
using Idigis.Web.States;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Idigis.Web
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            //Services
            builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);
            builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new("https://localhost:5004") });
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddBlazorState(options =>
            {
                options.Assemblies = new[] { typeof(Program).Assembly };
            });
            await builder.Build().RunAsync();
        }
    }
}
