using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<Response>> Login([FromBody] UserLoginModel model)
        {
            if (model == null)
                return Unauthorized(new LoginResponse() { Message = "No model passed" });
            if (!ModelState.IsValid)
                return Unauthorized(new LoginResponse()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            
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
        public async Task<ActionResult<Response>> Register([FromBody] UserRegistrationModel model)
        {
            if (model == null)
                return BadRequest(new Response { Errors = new List<string> { "No model passed" } });
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            var result = await _authenticationService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return Ok(result);
            }

            return Conflict(result);
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
