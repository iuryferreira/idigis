using System.Threading.Tasks;
using Idigis.Api.Auth.Contracts;
using Idigis.Core.Application.Contracts;
using Idigis.Shared.Dtos.Requests;
using Idigis.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Idigis.Api.Controllers
{
    [Route(Routes.Index.Base)]
    public class IndexController : ControllerBase
    {
        private readonly IChurchUseCase _usecase;
        private readonly IAuthService _authService;

        public IndexController (IChurchUseCase usecase, IAuthService authService)
        {
            _usecase = usecase;
            _authService = authService;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<CreateChurchResponse>> Signup ([FromBody] CreateChurchRequest request)
        {
            var response = await _usecase.Add(request);
            if (response is not null)
            {
                return Created($"{Request.GetDisplayUrl()}/{response.Id}", response);
            }

            return BadRequest(_usecase.Notificator.Notifications);
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<LoginResponse>> Signin ([FromBody] LoginRequest request)
        {

            var church = await _usecase.Get(new(request.Email));
            if (church is null)
            {
                return _usecase.Notificator.NotificationType.Name switch
                {
                    "NotFound" => NotFound(_usecase.Notificator.Notifications),
                    _ => StatusCode(500, _usecase.Notificator.Notifications)
                };
            }

            request.Hash = church.Password;
            request.ChurchId = church.Id;
            request.Name = church.Name;
            var response = _authService.Authenticate(request);
            if (response is not null)
            {
                return Ok(response);
            }
            return _usecase.Notificator.NotificationType.Name switch
            {
                "Unauthorized" => Unauthorized(_usecase.Notificator.Notifications),
                _ => StatusCode(500, _usecase.Notificator.Notifications)
            };

        }
    }
}
