using Application.Requests;
using Application.Responses;
using MediatR;

namespace Application.Handlers
{
    public interface ICreateChurchHandler : IRequestHandler<CreateChurch, CreateChurchResponse>
    {
    }
}
