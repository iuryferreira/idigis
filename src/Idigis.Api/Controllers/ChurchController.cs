using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Idigis.Api.Controllers
{
    [Route(Routes.Church.Base)]
    public class ChurchController : ControllerBase
    {
        private readonly IChurchUseCase _usecase;

        public ChurchController (IChurchUseCase usecase)
        {
            _usecase = usecase;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetChurchResponse>> Get ([FromRoute] string id)
        {
            var request = new GetChurchRequest("", id);
            var response = await _usecase.Get(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return _usecase.Notificator.NotificationType.Name switch
            {
                "NotFound" => NotFound(_usecase.Notificator.Notifications),
                _ => StatusCode(500, _usecase.Notificator.Notifications)
            };
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<EditChurchResponse>> Update ([FromBody] EditChurchRequest request,
            [FromRoute] string id)
        {
            request.Id = id;
            var response = await _usecase.Edit(request);
            if (response is not null)
            {
                return Ok(response);
            }

            return _usecase.Notificator.NotificationType.Name switch
            {
                "NotFound" => NotFound(_usecase.Notificator.Notifications),
                "Validation" => BadRequest(_usecase.Notificator.Notifications),
                _ => StatusCode(500, _usecase.Notificator.Notifications)
            };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeleteChurchResponse>> Delete ([FromRoute] string id)
        {
            var request = new DeleteChurchRequest(id);
            var response = await _usecase.Delete(request);
            if (response is not null)
            {
                return NoContent();
            }

            return _usecase.Notificator.NotificationType.Name switch
            {
                "NotFound" => NotFound(_usecase.Notificator.Notifications),
                _ => StatusCode(500, _usecase.Notificator.Notifications)
            };
        }
    }
}
