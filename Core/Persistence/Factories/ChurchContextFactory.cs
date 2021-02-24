using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;

namespace Persistence.Factories
{
    public class ChurchContextFactory : IDesignTimeDbContextFactory<ChurchContext>
    {
        public ChurchContext CreateDbContext (string[] args)
        {
            return CreateDbContext();
        }

        private static ChurchContext CreateDbContext ()
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddUserSecrets<ChurchContextFactory>()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ChurchContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("default"));

            return new ChurchContext(optionsBuilder.Options);
        }

    }
}
