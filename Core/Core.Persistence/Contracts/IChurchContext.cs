using System.Threading.Tasks;
using Core.Persistence.Models;
using Core.Shared.Types;

namespace Core.Persistence.Contracts
{
    public interface IChurchContext
    {
        Task<bool> Add (ChurchModel data);
        Task<bool> Exists (ChurchModel data);
        Task<bool> Update (ChurchModel data);
        Task<int> Save ();
        Task<ChurchModel> Get (Property property);

    }
}
