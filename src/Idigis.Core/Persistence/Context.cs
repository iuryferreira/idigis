using Idigis.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Idigis.Core.Persistence
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options) : base(options) { }

        internal DbSet<ChurchModel> ChurchContext { get; init; }
        internal DbSet<OfferModel> OfferContext { get; init; }
        internal DbSet<MemberModel> MemberContext { get; init; }
        internal DbSet<TitheModel> TitheContext { get; init; }
    }
}
