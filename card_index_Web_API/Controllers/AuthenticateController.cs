using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_Web_API.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Handles authentication tasks
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor, inject authentication service here
        /// </summary>
        /// <param name="authenticationService"></param>
        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Performs user login
        /// </summary>
        /// <param name="model">User login model</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<Response>> Login([FromBody] UserLoginModel model)
        {
            var result = await _authenticationService.LoginUserAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        /// <summary>
        /// Performs user registration in application
        /// </summary>
        /// <param name="model">User registration model</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        [Route("register")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<Response>> Register([FromBody] UserRegistrationModel model)
        {
            var result = await _authenticationService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Performs user sign out
        /// </summary>
        /// <returns>Task object</returns>
        [HttpPost]
        [Route("logout")]
        public async Task LogOut()
        {
            await _authenticationService.LogOutAsync();
        }
    }
}
