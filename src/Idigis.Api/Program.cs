using System;
using System.Diagnostics.CodeAnalysis;
using DotNetEnv;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Idigis.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main (string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder (string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        Env.TraversePath().Load();
                    }

                    configurationBuilder.AddEnvironmentVariables();
                });
        }
    }
}
