using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to users information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor, inject user service here
        /// </summary>
        /// <param name="userService">User service object</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns all users from DB
        /// </summary>
        /// <returns>All users collection</returns>
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

        /// <summary>
        /// Get user by specified id
        /// </summary>
        /// <param name="id">User id to search</param>
        /// <returns>User item</returns>
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

        /// <summary>
        /// Changes existing user info
        /// </summary>
        /// <param name="id">User id to change</param>
        /// <param name="model">New user object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("{id}")]
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

        /// <summary>
        /// Deletes existing user
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        /// <returns>Http status code of operation with response object</returns>
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

        /// <summary>
        /// Gets all roles from db
        /// </summary>
        /// <returns>All roles list</returns>
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

        /// <summary>
        /// Gets role by name
        /// </summary>
        /// <param name="roleName">Name of role to search</param>
        /// <returns>Role with specified name</returns>
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

        /// <summary>
        /// Adds new user role to DB
        /// </summary>
        /// <param name="model">Role object to add</param>
        /// <returns>Http status code of operation with response object</returns>
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

        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="roleName">Role name to delete</param>
        /// <returns>Http status code of operation with response object</returns>
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

        /// <summary>
        /// Gets currently logged in user for displaying in user cabinet
        /// </summary>
        /// <returns></returns>
        [HttpGet("cabinet")]
        [Authorize(Roles = "Admin,Moderator,Registered")]
        public async Task<ActionResult<UserInfoModel>> GetUserCabinet()
        {
            UserInfoModel user = null;
            var loggedInUser = this.User.FindFirstValue(ClaimTypes.Name);
            try
            {
                user = await _userService.GetByEmailAsync(loggedInUser);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Modifies currently logged in user
        /// </summary>
        /// <param name="model">User model for update</param>
        /// <returns></returns>
        [HttpPut("cabinet/modify")]
        [Authorize(Roles = "Admin,Moderator,Registered")]
        public async Task<ActionResult<Response>> ModifyUserCabinet([FromBody] UserInfoModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                var loggedInUser = this.User.FindFirstValue(ClaimTypes.Name);
                var user = await _userService.GetByEmailAsync(loggedInUser);
                model.UserRoles = user.UserRoles;
                model.Id = user.Id;
                await _userService.ModifyUserAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully updated user: {model.FirstName} {model.LastName}"));
        }
    }
}
