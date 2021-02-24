using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IChurchRepository
    {
        Task<bool> Add (Church entity);
    }
}
