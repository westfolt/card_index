using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Logic to work with user repository
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users info from db and maps to info model
        /// </summary>
        /// <returns>user info list</returns>
        Task<IEnumerable<UserInfoModel>> GetAllAsync();
        /// <summary>
        /// Gets user info for user with given id
        /// </summary>
        /// <param name="id">Id of user to search</param>
        /// <returns>Model, mapped from user</returns>
        Task<UserInfoModel> GetByIdAsync(int id);
        /// <summary>
        /// Gets user with given email
        /// </summary>
        /// <param name="email">Email address to search</param>
        /// <returns>User matching criteria</returns>
        Task<UserInfoModel> GetByEmailAsync(string email);
        /// <summary>
        /// Modifies user with given data,
        /// modifies only user data, not identity security values
        /// </summary>
        /// <param name="model">Model containing new user data</param>
        /// <returns>Response object, containing data about operation</returns>
        Task<Response> ModifyUserAsync(UserInfoModel model);
        /// <summary>
        /// Deletes user with given id
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        /// <returns>Response info about operation</returns>
        Task<Response> DeleteUserAsync(int id);
        /// <summary>
        /// Gets all user roles from DB
        /// </summary>
        /// <returns>List of existing user roles</returns>
        Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync();
        /// <summary>
        /// Gets role with given name from DB
        /// </summary>
        /// <param name="roleName">Role name to search</param>
        /// <returns>Role matching criteria</returns>
        Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName);
        /// <summary>
        /// Adds new role to DB
        /// </summary>
        /// <param name="model">model, containing data to add</param>
        /// <returns>Id of added role</returns>
        Task<int> AddRoleAsync(UserRoleInfoModel model);
        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="roleName">Role name to delete</param>
        /// <returns>Async operation</returns>
        Task DeleteRoleAsync(string roleName);
    }
}
