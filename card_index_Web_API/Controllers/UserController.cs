using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace card_index_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoModel>>> Get()
        {
            IEnumerable<UserInfoModel> users = null;

            try
            {
                users = await _userService.GetAllAsync();
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (users == null || !users.Any())
                return NotFound();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoModel>> GetById(int id)
        {
            UserInfoModel user = null;

            try
            {
                user = await _userService.GetByIdAsync(id);
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("id")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] UserInfoModel model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                await _userService.ModifyUserAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully updated user with id: {id}"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully deleted user with id: {id}"));
        }
    }
}
