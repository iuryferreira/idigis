using System.Diagnostics.CodeAnalysis;
using Idigis.Core.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Idigis.Tests.IntegrationTests.Persistence.Helpers
{
    [ExcludeFromCodeCoverage]
    public class TestContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext (string[] args)
        {
            return GenerateDbContext();
        }

        public static Context CreateDbContext ()
        {
            var context = GenerateDbContext();
            context.Database.Migrate();
            return context;
        }

        private static Context GenerateDbContext ()
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            SqliteConnection connection = new("Filename=:memory:");
            connection.Open();
            optionsBuilder.UseSqlite(connection, c => c.MigrationsAssembly("Idigis.Tests"));
            return new(optionsBuilder.Options);
        }
    }
}
