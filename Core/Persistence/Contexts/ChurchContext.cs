using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.Contexts
{
    public class ChurchContext : DbContext
    {
        public ChurchContext (DbContextOptions<ChurchContext> options) : base(options) { }

        public DbSet<ChurchModel> Churches { get; set; }

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
