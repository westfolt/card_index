using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Response> LoginUser(UserLoginModel model)
        {
            try
            {
                var exists = await _userManager.Users.AnyAsync(u => u.Email == model.Email);
                if (!exists)
                    return new Response(false, $"No user with email: {model.Email}");

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded)
                    return new Response(true, $"Logged in as {model.Email}");

                return new Response(false, $"Cannot login {model.Email}. Wrong email or password.");
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
                return new Response() { Message = $"Email {model.Email} is already taken" };

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
                    return new Response(true, $"User {model.FirstName} {model.LastName} successfully added");

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
