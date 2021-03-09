using System.Threading.Tasks;
using Core.Persistence.Models;

namespace Core.Persistence.Contracts
{
    public interface IChurchContext
    {
        Task<bool> Add (ChurchModel data);
        Task<bool> Exists (ChurchModel data);
        Task<int> Save ();
        Task<ChurchModel> Get (string property, string value);
    }
}
