using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace card_index_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            throw new NotImplementedException();
            //TODO
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            var alreadyExists = _userService.GetByEmailAsync(model.Email);
            if(alreadyExists != null)
                return StatusCode(StatusCodes.Status409Conflict, )
        }
    }
}
