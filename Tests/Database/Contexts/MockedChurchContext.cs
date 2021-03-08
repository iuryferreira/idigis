using System.Threading.Tasks;
using Core.Persistence.Contracts;
using Core.Persistence.Models;

namespace Tests.Database.Contexts
{
    public class MockedChurchContext : IChurchContext
    {
        public Task<bool> Add (ChurchModel data)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Exists (ChurchModel data)
        {
            return Task.FromResult(false);
        }

        public Task<int> Save ()
        {
            return Task.FromResult(1);
        }
    }
}
