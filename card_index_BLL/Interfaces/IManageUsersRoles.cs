using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    /// <summary>
    /// Wrapping class for identity managers
    /// </summary>
    public interface IManageUsersRoles
    {
        /// <summary>
        /// Gets Users property from UserManager
        /// </summary>
        /// <returns>Users list</returns>
        Task<List<User>> GetUsersUMAsync();
        /// <summary>
        /// Gets roles for user from UserManager
        /// </summary>
        /// <param name="user">User to search for roles for</param>
        /// <returns>string list with role names</returns>
        Task<IList<string>> GetRolesFromUserManagerAsync(User user);
        /// <summary>
        /// Searches for user by email
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User entity</returns>
        Task<User> FindByEmailAsync(string email);
        /// <summary>
        /// Searches for user by username
        /// </summary>
        /// <param name="userName">Users name</param>
        /// <returns>User entity</returns>
        Task<User> FindByNameAsync(string userName);
        /// <summary>
        /// Removes user from given roles
        /// </summary>
        /// <param name="user">User entity to edit</param>
        /// <param name="roles">List of roles to remove</param>
        /// <returns>Identity result object</returns>
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        /// <summary>
        /// Creates new user 
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="password">Password for user</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> CreateUserAsync(User user, string password);
        /// <summary>
        /// Adds user to given role
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="role">Role to add to</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        /// <summary>
        /// Updates user in DB
        /// </summary>
        /// <param name="user">User to modify</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> UpdateUserAsync(User user);
        /// <summary>
        /// Deletes user from DB
        /// </summary>
        /// <param name="user">User object to remove</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> DeleteUserAsync(User user);
        /// <summary>
        /// Changes users password
        /// </summary>
        /// <param name="user">User, who's password you want to change</param>
        /// <param name="currentPassword">current user's password</param>
        /// <param name="newPassword">new user's password</param>
        /// <returns></returns>
        Task<IdentityResult> ChangeUserPasswordAsync(User user, string currentPassword, string newPassword);
        /// <summary>
        /// Checks, if given password is valid for given user
        /// </summary>
        /// <param name="user">The user whose password should be validated</param>
        /// <param name="password">The password to validate</param>
        /// <returns>True, if password valid for this user, false otherwise</returns>
        Task<bool> CheckPasswordAsync(User user, string password);
        /// <summary>
        /// Gets roles list from Rolemanager
        /// </summary>
        /// <returns>List containing user roles</returns>
        Task<List<UserRole>> GetRolesFromRoleManagerAsync();
        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="role">User role to add</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> CreateRoleAsync(UserRole role);
        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="role">User role to delete</param>
        /// <returns>Identity result</returns>
        Task<IdentityResult> DeleteRoleAsync(UserRole role);
        /// <summary>
        /// Performs sign in
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">users password</param>
        /// <param name="isPersistent">stay authenticated</param>
        /// <param name="lockoutOnFailure">lockout if sigh in failure</param>
        /// <returns>SignIn result</returns>
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure);
        /// <summary>
        /// Signs user out
        /// </summary>
        /// <returns>Async operation</returns>
        Task SignOutAsync();
        /// <summary>
        /// Gets UserManager entity
        /// </summary>
        /// <returns>UserManager</returns>
        UserManager<User> GetUserManager();
        /// <summary>
        /// Gets RoleManager entity
        /// </summary>
        /// <returns>RoleManager</returns>
        RoleManager<UserRole> GetRoleManager();
        /// <summary>
        /// Gets SignInManager entity
        /// </summary>
        /// <returns>SignInManager</returns>
        SignInManager<User> GetSignInManager();

    }
}
