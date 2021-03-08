using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;

namespace Core.Application.Handlers
{
    public interface ICreateChurchHandler : IRequestHandler<CreateChurch, CreateChurchResponse>{ }
}
