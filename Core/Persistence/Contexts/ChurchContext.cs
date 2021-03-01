using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Contracts;
using Persistence.Models;

namespace Persistence.Contexts
{
    public sealed class ChurchContext : DbContext, IChurchContext
    {
        public ChurchContext (DbContextOptions<ChurchContext> options) : base(options)
        {
            if (!this.Database.CanConnect())
                throw new("Could not connect to the database.");
        }

        private DbSet<ChurchModel> Entity { get; set; }

        public async Task<bool> Add (ChurchModel data)
        {
            var entry = await Entity.AddAsync(data);
            try
            {
                await this.Save();
                return entry.State is EntityState.Unchanged;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Exists (ChurchModel data)
        {
            ChurchModel entity = await Entity.Where(e => e.Email == data.Email || e.Id == data.Id).FirstOrDefaultAsync();
            return entity is not null;
        }

        public async Task<int> Save ()
        {
            return await this.SaveChangesAsync();
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
