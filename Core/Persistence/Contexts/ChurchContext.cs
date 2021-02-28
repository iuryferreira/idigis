using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contracts;
using Persistence.Models;

namespace Persistence.Contexts
{
    public class ChurchContext : DbContext, IChurchContext
    {
        public ChurchContext (DbContextOptions<ChurchContext> options) : base(options)
        {
            if (!this.Database.CanConnect())
                throw new Exception("Could not connect to the database.");
        }

        private DbSet<ChurchModel> Entity { get; set; }

        public async Task<bool> Add (ChurchModel data)
        {
            var entry = await Entity.AddAsync(data);
            await this.SaveChangesAsync();
            return entry.State is EntityState.Unchanged;
        }

        public async Task<bool> Exists (ChurchModel data)
        {
            ChurchModel entity = await Entity.Where(e => e.Email == data.Email || e.Id == data.Id).FirstOrDefaultAsync();
            return entity is not null;
        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ChurchTypeConfiguration());
        }
    }

    internal class ChurchTypeConfiguration : IEntityTypeConfiguration<ChurchModel>
    {
        public void Configure (EntityTypeBuilder<ChurchModel> builder)
        {
            builder.ToTable("churches");
        }
    }
}
