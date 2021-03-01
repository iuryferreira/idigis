using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Contracts
{
    public interface IChurchContext
    {
        Task<bool> Add (ChurchModel data);
        Task<bool> Exists (ChurchModel data);
        Task<int> Save ();
    }
}
