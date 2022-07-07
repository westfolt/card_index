using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_BLL.Security;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    /// <summary>
    /// Implements interface between webapi and repository
    /// Performs authentication tasks
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        //private readonly SignInManager<User> _signInManager;
        private readonly IManageUsersRoles _usersRolesManager;
        private readonly JwtHandler _jwtHandler;

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="usersRolesManager">Object, wrapping userManager, roleManager and signInManager</param>
        /// <param name="jwtHandler">Handler of jwt creation</param>
        public AuthenticationService(IManageUsersRoles usersRolesManager, JwtHandler jwtHandler)
        {
            _usersRolesManager = usersRolesManager;
            //_signInManager = usersRolesManager.GetSignInManager();
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Performs user login, responds with object, containing
        /// JWT, if login successful
        /// </summary>
        /// <param name="model">user login model with email and pass</param>
        /// <returns>Login response, containing info, errors, token, operation result</returns>
        /// <exception cref="CardIndexException">Thrown if problems during login process</exception>
        public async Task<LoginResponse> LoginUserAsync(UserLoginModel model)
        {
            try
            {
                var user = await _usersRolesManager.FindByNameAsync(model.Email);

                if (user == null)
                    return new LoginResponse { Errors = new List<string> { $"No user with email: {model.Email}" } };

                var result = await _usersRolesManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (!result.Succeeded)
                    return new LoginResponse { Errors = new List<string> { "Wrong login or password" } };

                //successfully authenticated
                var token = await _jwtHandler.GenerateJwt(user);
                return new LoginResponse(true, $"Successfully logged in {model.Email}", token);
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Error during login process", ex);
            }
        }

        /// <summary>
        /// Handles user registration process, returns response
        /// object with operation result
        /// </summary>
        /// <param name="model">registration model with info to create user</param>
        /// <returns>Operation result</returns>
        /// <exception cref="CardIndexException">Thrown if problems during registration process</exception>
        public async Task<Response> RegisterUserAsync(UserRegistrationModel model)
        {
            var alreadyExists = await _usersRolesManager.FindByEmailAsync(model.Email);
            if (alreadyExists != null)
                return new Response() { Message = "Cannot create user", Errors = new List<string> { $"Email {model.Email} is already taken" } };

            try
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.Phone
                };
                var result = await _usersRolesManager.CreateUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _usersRolesManager.AddUserToRoleAsync(user, "Registered");
                    return new Response(true, $"User {model.FirstName} {model.LastName} successfully added");
                }

                return new Response { Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add new user", ex);
            }
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <returns>Async operation</returns>
        public async Task LogOutAsync()
        {
            await _usersRolesManager.SignOutAsync();
        }
    }
}
