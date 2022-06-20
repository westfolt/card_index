using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;

namespace card_index_BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response> LoginUser(UserLoginModel model);
        Task<Response> RegisterUser(UserRegistrationModel model);
        Task LogOut();
    }
}
