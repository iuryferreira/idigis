using System.Threading.Tasks;
using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace External.Server.Controllers
{
    [Route("api/")]
    public class IndexController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IndexController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponse>> Signin ([FromBody] LoginRequest request)
        {
            var response = await _mediator.Send(request);
            if (response is not null)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<CreateChurchResponse>> Signup ([FromBody] CreateChurchRequest request)
        {
            var response = await _mediator.Send(request);
            if (response is not null)
            {
                return Created($"{Request.GetDisplayUrl()}/{response.Id}", response);
            }
            return BadRequest();
        }

    }
}
