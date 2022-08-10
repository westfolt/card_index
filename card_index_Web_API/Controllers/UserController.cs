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
using card_index_Web_API.Filters;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to users information
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator,Registered")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserInfoModel>>> Get()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        /// <summary>
        /// Get user by specified id
        /// </summary>
        /// <param name="id">User id to search</param>
        /// <returns>User item</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserInfoModel>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Changes existing user info
        /// </summary>
        /// <param name="id">User id to change</param>
        /// <param name="model">New user object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(UserValidationFilter))]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] UserInfoModel model)
        {
            model.Id = id;
            await _userService.ModifyUserAsync(model);
            return Ok(new Response(true, $"Successfully updated user with id: {id}"));
        }

        /// <summary>
        /// Deletes existing user
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(new Response(true, $"Successfully deleted user with id: {id}"));
        }

        /// <summary>
        /// Gets all roles from db
        /// </summary>
        /// <returns>All roles list</returns>
        [HttpGet("roles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserRoleInfoModel>>> GetRoles()
        {
            var roles = await _userService.GetAllRolesAsync();
            if (roles == null || !roles.Any())
            {
                return NotFound();
            }

            return Ok(roles);
        }

        /// <summary>
        /// Gets role by name
        /// </summary>
        /// <param name="roleName">Name of role to search</param>
        /// <returns>Role with specified name</returns>
        [HttpGet("roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserRoleInfoModel>> GetRoleByName(string roleName)
        {
            var role = await _userService.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        /// <summary>
        /// Adds new user role to DB
        /// </summary>
        /// <param name="model">Role object to add</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost("roles")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(UserValidationFilter))]
        public async Task<ActionResult<Response>> AddRole([FromBody] UserRoleInfoModel model)
        {
            var insertId = await _userService.AddRoleAsync(model);
            return Ok(new Response(true, $"Successfully added role with id: {insertId}"));
        }

        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="roleName">Role name to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Delete(string roleName)
        {
            await _userService.DeleteRoleAsync(roleName);
            return Ok(new Response(true, $"Successfully deleted role {roleName}"));
        }

        /// <summary>
        /// Gets currently logged in user for displaying in user cabinet
        /// </summary>
        /// <returns>Http status code of operation with response object</returns>
        [HttpGet("cabinet")]
        public async Task<ActionResult<UserInfoModel>> GetUserCabinet()
        {
            var loggedInUser = this.User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetByEmailAsync(loggedInUser);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Modifies currently logged in user
        /// </summary>
        /// <param name="model">User model for update</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("cabinet/modify")]
        [ServiceFilter(typeof(UserValidationFilter))]
        public async Task<ActionResult<Response>> ModifyUserCabinet([FromBody] UserInfoModel model)
        {
            var loggedInUser = this.User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetByEmailAsync(loggedInUser);
            model.UserRoles = user.UserRoles;
            model.Id = user.Id;
            await _userService.ModifyUserAsync(model);

            return Ok(new Response(true, $"Successfully updated user: {model.FirstName} {model.LastName}"));
        }
        /// <summary>
        /// Changes password for user, currently logged in
        /// </summary>
        /// <param name="changePassword">change pass model, contains current and new passwords</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost("cabinet/changepass")]
        [ServiceFilter(typeof(UserValidationFilter))]
        public async Task<ActionResult<Response>> ChangeUserPassword([FromBody] ChangePasswordModel changePassword)
        {
            var loggedInUser = this.User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetByEmailAsync(loggedInUser);
            var passValid = await _userService.CheckPasswordAsync(user, changePassword.CurrentPassword);
            if (!passValid)
            {
                return BadRequest(new Response() { Errors = new List<string> { "Current password is wrong" } });
            }

            var result = await _userService.ChangeUserPasswordAsync(user, changePassword.CurrentPassword,
                changePassword.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new Response()
                    { Errors = result.Errors, Message = "Error while trying to change password" });
            }

            return Ok(new Response(true,
                $"Successfully changed pass for user: {user.FirstName} {user.LastName}"));
        }
    }
}
