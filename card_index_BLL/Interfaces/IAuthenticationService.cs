using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Logic to handle authentication
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Performs user login, responds with object, containing
        /// JWT, if login successful
        /// </summary>
        /// <param name="model">user login model with email and pass</param>
        /// <returns>Login response, containing info, errors, token, operation result</returns>
        Task<LoginResponse> LoginUser(UserLoginModel model);
        /// <summary>
        /// Handles user registration process, returns response
        /// object with operation result
        /// </summary>
        /// <param name="model">registration model with info to create user</param>
        /// <returns>Operation result</returns>
        Task<Response> RegisterUser(UserRegistrationModel model);
        /// <summary>
        /// Logout user
        /// </summary>
        /// <returns>Async operation</returns>
        Task LogOut();
    }
}
