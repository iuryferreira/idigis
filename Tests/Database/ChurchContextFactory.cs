using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;

namespace Tests.Database.Factories
{
#nullable enable
    public class ChurchContextFactoryForTest : IDesignTimeDbContextFactory<ChurchContext>
    {
        public ChurchContext CreateDbContext (string[] args)
        {
            return GenerateDbContext();
        }

        public static ChurchContext CreateDbContext (string? connectionString = null)
        {
            var context = GenerateDbContext(connectionString);
            context.Database.Migrate();
            return context;
        }

        private static ChurchContext GenerateDbContext (string? connectionString = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ChurchContext>();
            var connection = new SqliteConnection(connectionString ?? "Filename=:memory:");
            connection.Open();
            optionsBuilder.UseSqlite(connection, o => o.MigrationsAssembly("Tests"));
            return new(optionsBuilder.Options);
        }
    }
}
