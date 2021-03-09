using Core.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tests.Persistence.Database.Factories
{
#nullable enable
    public class ChurchContextFactoryForTests : IDesignTimeDbContextFactory<ChurchContext>
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
            var optionsBuilder = new DbContextOptionsBuilder<ChurchContext>();
            SqliteConnection connection = new("Filename=:memory:");
            connection.Open();
            if (connectionString is not null)
            {
                optionsBuilder.UseSqlite(connectionString, c => c.MigrationsAssembly("Tests.Persistence"));
            }
            else
            {
                optionsBuilder.UseSqlite(connection, c => c.MigrationsAssembly("Tests.Persistence"));
            }

            return new(optionsBuilder.Options);
        }
    }
}
