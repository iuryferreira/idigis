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
    [Route(Routes.Offer.Base)]
    public class OfferController : ControllerBase
    {
        private readonly IOfferUseCase _usecase;

        public OfferController (IOfferUseCase usecase)
        {
            _usecase = usecase;
        }

        [HttpPost]
        public async Task<ActionResult<CreateOfferResponse>> Store ([FromBody] CreateOfferRequest request)
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
        public async Task<ActionResult<List<GetOfferResponse>>> List ([FromQuery] string churchId)
        {
            var request = new ListOfferRequest(churchId);
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
        public async Task<ActionResult<GetOfferResponse>> Get ([FromQuery] string churchId, [FromRoute] string id)
        {
            var request = new GetOfferRequest(churchId, id);
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
        public async Task<ActionResult<EditOfferResponse>> Update ([FromRoute] string id,
            [FromBody] EditOfferRequest request)
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
        public async Task<ActionResult<DeleteOfferResponse>> Delete ([FromQuery] string churchId, [FromRoute] string id)
        {
            var request = new DeleteOfferRequest(churchId, id);
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
