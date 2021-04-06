using System.Threading.Tasks;
using Idigis.Core.Domain.Dtos.Requests;
using Idigis.Core.Domain.Dtos.Responses;

namespace Idigis.Core.Application
{
    public interface IChurchUseCase
    {
        public Task<CreateChurchResponse> Add (CreateChurchRequest data);
    }
}
