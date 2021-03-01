using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;

namespace Persistence.Factories
{
#nullable enable
    public class ChurchContextFactory : IDesignTimeDbContextFactory<ChurchContext>
    {
        [ExcludeFromCodeCoverage]
        public ChurchContext CreateDbContext (string[] args)
        {
            return GenerateDbContext();
        }

        public static ChurchContext CreateDbContext (string? connectionString = null)
        {
            return GenerateDbContext(connectionString);
        }

        private static ChurchContext GenerateDbContext (string? connectionString = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<ChurchContextFactory>()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChurchContext>();
            optionsBuilder.UseSqlServer(connectionString ?? configuration.GetConnectionString("default"));
            return new(optionsBuilder.Options);
        }
    }
}
