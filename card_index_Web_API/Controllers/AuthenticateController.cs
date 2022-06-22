﻿using card_index_BLL.Exceptions;
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

        /// <summary>
        /// Performs user sign out
        /// </summary>
        /// <returns>Task object</returns>
        [HttpPost]
        public async Task LogOut()
        {
            await _authenticationService.LogOut();
        }
    }
}
