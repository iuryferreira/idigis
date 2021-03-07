using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistence.Contexts;

namespace Tests.Database.Factories
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
                optionsBuilder.UseSqlite(connectionString, c => c.MigrationsAssembly("Tests"));
            }
            else
            {
                optionsBuilder.UseSqlite(connection, c => c.MigrationsAssembly("Tests"));
            }

            return new(optionsBuilder.Options);
        }
    }
}
