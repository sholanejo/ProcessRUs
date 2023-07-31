using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcessRUs.Application.Queries;
using ProcessRUs.Helpers;
using System.Net;

namespace ProcessRUs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin + "," + UserRoles.BackOffice}")]
    public class AccessController : ControllerBase
    {
        private readonly ISender _sender;

        public AccessController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Endpoint that spits out fruits
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
        [HttpGet]
        [Route("fruits")]
        public async Task<IActionResult> GetFruits([FromQuery] GetFruitsQuery query)
        {
            var result = await _sender.Send(query);
            return Ok(result);
        }

    }
}
