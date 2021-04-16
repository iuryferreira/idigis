using System.Collections.Generic;
using System.Threading.Tasks;
using Idigis.Core.Application.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Idigis.Api.Controllers
{
    [ApiController]
    [Route(Routes.Member.Base)]
    public class MemberController : ControllerBase
    {
        private readonly IMemberUseCase _usecase;

        public MemberController (IMemberUseCase usecase)
        {
            _usecase = usecase;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreateMemberResponse>> Store ([FromBody] CreateMemberRequest request)
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetMemberResponse>>> List ([FromQuery] string churchId)
        {
            var request = new ListMemberRequest(churchId);
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

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetMemberResponse>> Get ([FromQuery] string churchId, [FromRoute] string id)
        {
            var request = new GetMemberRequest(churchId, id);
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

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<EditMemberResponse>> Update ([FromRoute] string id,
            [FromBody] EditMemberRequest request)
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

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<DeleteMemberResponse>> Delete ([FromQuery] string churchId,
            [FromRoute] string id)
        {
            var request = new DeleteMemberRequest(churchId, id);
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
