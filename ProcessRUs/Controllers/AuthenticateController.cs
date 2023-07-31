using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcessRUs.Application.Commands;
using System.Net;

namespace ProcessRUs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthenticateController(ISender sender)
        {
            _sender = sender;
        }



        /// <summary>
        /// Login to generate auth token
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(LoginResult), (int)HttpStatusCode.BadRequest)]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand model)
        {
            var result = await _sender.Send(model);
            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}