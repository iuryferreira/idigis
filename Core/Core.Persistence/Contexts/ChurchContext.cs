using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Persistence.Contracts;
using Core.Persistence.Models;
using Core.Shared.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Persistence.Contexts
{
    public sealed class ChurchContext : DbContext, IChurchContext
    {
        public ChurchContext (DbContextOptions<ChurchContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                throw new("Could not connect to the database.");
            }
        }

        private DbSet<ChurchModel> Entity { get; set; }

        public async Task<bool> Add (ChurchModel data)
        {
            var entry = await Entity.AddAsync(data);
            try
            {
                await Save();
                return entry.State is EntityState.Unchanged;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Exists (ChurchModel data)
        {
            var model =
                await Entity.Where(e => e.Email == data.Email || e.Id == data.Id).FirstOrDefaultAsync();
            return model is not null;
        }

        public async Task<int> Save ()
        {
            return await SaveChangesAsync();
        }

        public async Task<ChurchModel> Get (Property property)
        {
            var model = await Entity.Where(e => e.Email == property.Value || e.Id == property.Value)
                .FirstOrDefaultAsync();
            return model;
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChurchTypeConfiguration());
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
