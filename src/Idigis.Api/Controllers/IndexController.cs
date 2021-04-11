using System.Threading.Tasks;
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
        private readonly IChurchUseCase _churchUseCase;

        public IndexController (IChurchUseCase churchUseCase)
        {
            _churchUseCase = churchUseCase;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<CreateChurchResponse>> Signup ([FromBody] CreateChurchRequest request)
        {
            var response = await _churchUseCase.Add(request);
            if (response is not null)
            {
                return Created($"{Request.GetDisplayUrl()}/{response.Id}", response);
            }

            return BadRequest(_churchUseCase.Notificator.Notifications);
        }
    }
}
