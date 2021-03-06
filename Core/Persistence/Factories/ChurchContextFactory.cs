using System.Diagnostics.CodeAnalysis;
using System.IO;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;

namespace Persistence.Factories
{
#nullable enable
    [ExcludeFromCodeCoverage]
    public class ChurchContextFactory : IDesignTimeDbContextFactory<ChurchContext>
    {
        public ChurchContext CreateDbContext (string[] args)
        {
            return GenerateDbContext();
        }

        private static ChurchContext GenerateDbContext ()
        {
            Env.TraversePath().Load();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChurchContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
            return new(optionsBuilder.Options);
        }
    }
}
