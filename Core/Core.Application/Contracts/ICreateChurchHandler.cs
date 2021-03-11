using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;

namespace Core.Application.Contracts
{
    public interface ICreateChurchHandler : IRequestHandler<CreateChurchRequest, CreateChurchResponse>
    {
    }
}
