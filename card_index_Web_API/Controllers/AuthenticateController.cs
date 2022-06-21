using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace card_index_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<Response>> Login([FromBody] UserLoginModel model)
        {
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                var result = await _authenticationService.LoginUser(model);
                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status403Forbidden, result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Response>> Register([FromBody] UserRegistrationModel model)
        {
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                var result = await _authenticationService.RegisterUser(model);
                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return Conflict(result);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        public async Task LogOut()
        {
            await _authenticationService.LogOut();
        }
    }
}
