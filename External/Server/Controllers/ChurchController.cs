using System;
using System.Threading.Tasks;
using Application.Requests;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/churches")]
    [ApiController]
    public class ChurchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChurchController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateChurchResponse>> Store ([FromBody] CreateChurch request)
        {
            Console.WriteLine(request.Password);
            var response = await _mediator.Send(request);
            if (response is not null)
            {
                return Created($"{Request.GetDisplayUrl()}/{response.Id}", response);
            }

            return BadRequest();
        }
    }
}
