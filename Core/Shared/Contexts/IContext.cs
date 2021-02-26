using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Contexts
{
    public interface IContext
    {
        Task<bool> Save ();
        Task<bool> Add<T> (T data) where T : Model;
    }
}
