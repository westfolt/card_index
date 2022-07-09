using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class FakeAuthenticationService:IAuthenticationService
    {
        public Task<LoginResponse> LoginUserAsync(UserLoginModel model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<Response> RegisterUserAsync(UserRegistrationModel model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task LogOutAsync()
        {
            throw new CardIndexException("Something went wrong");
        }
    }
}
