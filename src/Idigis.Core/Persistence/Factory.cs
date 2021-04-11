using System.Diagnostics.CodeAnalysis;
using System.IO;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Idigis.Core.Persistence
{
    [ExcludeFromCodeCoverage]
    public class Factory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext (string[] args)
        {
            return GenerateDbContext();
        }

        private static Context GenerateDbContext ()
        {
            Env.TraversePath().Load();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
            return new(optionsBuilder.Options);
        }
    }
}
