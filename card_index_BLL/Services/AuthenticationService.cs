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
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtHandler _jwtHandler;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHandler = jwtHandler;
        }

        public async Task<LoginResponse> LoginUser(UserLoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                    return new LoginResponse { Errors = new List<string> { $"No user with email: {model.Email}" } };

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

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

        public async Task<Response> RegisterUser(UserRegistrationModel model)
        {
            var alreadyExists = await _userManager.FindByEmailAsync(model.Email);
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
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Registered");
                    return new Response(true, $"User {model.FirstName} {model.LastName} successfully added");
                }

                return new Response { Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add new user", ex);
            }
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
