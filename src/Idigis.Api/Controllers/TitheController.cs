using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Idigis.Api.Controllers
{
    [ApiController]
    [Route(Routes.Tithe.Base)]
    public class TitheController : ControllerBase
    {
        private readonly ITitheUseCase _usecase;

        public TitheController (ITitheUseCase usecase)
        {
            _usecase = usecase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateTitheResponse>> Store ([FromBody] CreateTitheRequest request)
        {
            var response = await _usecase.Add(request);
            if (response is not null)
            {
                return Created($"{Request.GetDisplayUrl()}/{response.Id}", response);
            }

            return _usecase.Notificator.NotificationType.Name switch
            {
                "Validation" => BadRequest(_usecase.Notificator.Notifications),
                _ => StatusCode(500, _usecase.Notificator.Notifications)
            };
        }

        [HttpGet]
        public async Task<ActionResult<List<GetTitheResponse>>> List ([FromQuery] string churchId)
        {
            var request = new ListTitheRequest(churchId);
            var response = await _usecase.List(request);
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

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetTitheResponse>> Get ([FromQuery] string churchId, [FromQuery] string memberId,
            [FromRoute] string id)
        {
            var request = new GetTitheRequest(churchId, memberId, id);
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
        public async Task<ActionResult<EditTitheResponse>> Update ([FromRoute] string id,
            [FromBody] EditTitheRequest request)
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
        public async Task<ActionResult<DeleteTitheResponse>> Delete ([FromQuery] string churchId,
            [FromQuery] string memberId, [FromRoute] string id)
        {
            var request = new DeleteTitheRequest(churchId, memberId, id);
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
