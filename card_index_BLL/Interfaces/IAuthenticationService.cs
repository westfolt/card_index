using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginUser(UserLoginModel model);
        Task<Response> RegisterUser(UserRegistrationModel model);
        Task LogOut();
    }
}
