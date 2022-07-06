using card_index_BLL.Interfaces;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Infrastructure
{
    /// <summary>
    /// Wrapping class for identity managers
    /// </summary>
    public class ManageUsersRoles : IManageUsersRoles
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Takes in Identity managers
        /// </summary>
        /// <param name="userManager">User manager with user type</param>
        /// <param name="roleManager">Role manager with users role type</param>
        /// <param name="signInManager">Manager for sign in operations</param>
        public ManageUsersRoles(UserManager<User> userManager, RoleManager<UserRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Gets Users property from UserManager
        /// </summary>
        /// <returns>Users list</returns>
        public async Task<List<User>> GetUsersUMAsync() => await _userManager.Users.ToListAsync();
        /// <summary>
        /// Gets roles for user from UserManager
        /// </summary>
        /// <param name="user">User to search for roles for</param>
        /// <returns>string list with role names</returns>
        public async Task<IList<string>> GetRolesFromUserManagerAsync(User user) => await _userManager.GetRolesAsync(user);
        /// <summary>
        /// Searches for user by email
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>User entity</returns>
        public async Task<User> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        /// <summary>
        /// Searches for user by username
        /// </summary>
        /// <param name="userName">Users name</param>
        /// <returns>User entity</returns>
        public async Task<User> FindByNameAsync(string userName) => await _userManager.FindByNameAsync(userName);
        /// <summary>
        /// Removes user from given roles
        /// </summary>
        /// <param name="user">User entity to edit</param>
        /// <param name="roles">List of roles to remove</param>
        /// <returns>Identity result object</returns>
        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles) =>
            await _userManager.RemoveFromRolesAsync(user, roles);
        /// <summary>
        /// Creates new user 
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="password">Password for user</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> CreateUserAsync(User user, string password) =>
            await _userManager.CreateAsync(user, password);
        /// <summary>
        /// Adds user to given role
        /// </summary>
        /// <param name="user">User object</param>
        /// <param name="role">Role to add to</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> AddUserToRoleAsync(User user, string role) =>
            await _userManager.AddToRoleAsync(user, role);
        /// <summary>
        /// Updates user in DB
        /// </summary>
        /// <param name="user">User to modify</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> UpdateUserAsync(User user) =>
            await _userManager.UpdateAsync(user);
        /// <summary>
        /// Deletes user from DB
        /// </summary>
        /// <param name="user">User object to remove</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> DeleteUserAsync(User user) =>
            await _userManager.DeleteAsync(user);
        /// <summary>
        /// Changes users password
        /// </summary>
        /// <param name="user">User, who's password you want to change</param>
        /// <param name="currentPassword">current user's password</param>
        /// <param name="newPassword">new user's password</param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangeUserPasswordAsync(User user, string currentPassword, string newPassword) =>
            await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        /// <summary>
        /// Checks, if given password is valid for given user
        /// </summary>
        /// <param name="user">The user whose password should be validated</param>
        /// <param name="password">The password to validate</param>
        /// <returns>True, if password valid for this user, false otherwise</returns>
        public async Task<bool> CheckPasswordAsync(User user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        /// <summary>
        /// Gets roles list from Rolemanager
        /// </summary>
        /// <returns>List containing user roles</returns>
        public async Task<List<UserRole>> GetRolesFromRoleManagerAsync() => await _roleManager.Roles.ToListAsync();
        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="role">User role to add</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> CreateRoleAsync(UserRole role) => await _roleManager.CreateAsync(role);
        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="role">User role to delete</param>
        /// <returns>Identity result</returns>
        public async Task<IdentityResult> DeleteRoleAsync(UserRole role) => await _roleManager.DeleteAsync(role);
        /// <summary>
        /// Performs sign in
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">users password</param>
        /// <param name="isPersistent">stay authenticated</param>
        /// <param name="lockoutOnFailure">lockout if sigh in failure</param>
        /// <returns>SignIn result</returns>
        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure)
            => await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        /// <summary>
        /// Signs user out
        /// </summary>
        /// <returns>Async operation</returns>
        public async Task SignOutAsync() => await _signInManager.SignOutAsync();

        /// <summary>
        /// Gets UserManager entity
        /// </summary>
        /// <returns>UserManager</returns>
        public UserManager<User> GetUserManager() => _userManager;
        /// <summary>
        /// Gets RoleManager entity
        /// </summary>
        /// <returns>RoleManager</returns>
        public RoleManager<UserRole> GetRoleManager() => _roleManager;
        /// <summary>
        /// Gets SignInManager entity
        /// </summary>
        /// <returns>SignInManager</returns>
        public SignInManager<User> GetSignInManager() => _signInManager;
    }
}
