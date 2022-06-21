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

        [HttpPut("addrole/{id}/{newRole}")]
        public async Task<ActionResult<Response>> AddRole(int id, string newRole)
        {
            try
            {
                await _userService.AddRoleToUserAsync(id, newRole);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully added role {newRole} to user with id: {id}"));
        }

        [HttpPut("removerole/{id}/{roleToRemove}")]
        public async Task<ActionResult<Response>> RemoveRole(int id, string roleToRemove)
        {
            try
            {
                await _userService.RemoveRoleFromUserAsync(id, roleToRemove);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully removed role {roleToRemove} from user with id: {id}"));
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<UserRoleInfoModel>>> GetRoles()
        {
            IEnumerable<UserRoleInfoModel> roles = null;

            try
            {
                roles = await _userService.GetAllRolesAsync();
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (roles == null || !roles.Any())
                return NotFound();

            return Ok(roles);
        }

        [HttpGet("roles/{roleName}")]
        public async Task<ActionResult<UserRoleInfoModel>> GetRoleByName(string roleName)
        {
            UserRoleInfoModel role = null;

            try
            {
                role = await _userService.GetRoleByNameAsync(roleName);
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost("roles")]
        public async Task<ActionResult<Response>> AddRole([FromBody] UserRoleInfoModel model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                insertId = await _userService.AddRoleAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully added role with id: {insertId}"));
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<ActionResult<Response>> Delete(string roleName)
        {
            try
            {
                await _userService.DeleteRoleAsync(roleName);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully deleted role {roleName}"));
        }
    }
}
