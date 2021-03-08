using System.Diagnostics.CodeAnalysis;
using DotNetEnv;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace External.Server
{
    public static class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main (string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder (string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    Env.TraversePath().Load();
                    configurationBuilder.AddEnvironmentVariables();
                });
        }
    }
}
