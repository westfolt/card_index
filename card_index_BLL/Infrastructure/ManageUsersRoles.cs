using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Interfaces;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace card_index_BLL.Infrastructure
{
    public class ManageUsersRoles:IManageUsersRoles
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public ManageUsersRoles(UserManager<User> userManager, RoleManager<UserRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<List<User>> GetUsersUMAsync() => await _userManager.Users.ToListAsync();
        public async Task<IList<string>> GetRolesFromUserManagerAsync(User user) => await _userManager.GetRolesAsync(user);

        public async Task<User> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<User> FindByNameAsync(string userName) => await _userManager.FindByNameAsync(userName);
        public async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles) =>
            await _userManager.RemoveFromRolesAsync(user, roles);

        public async Task<IdentityResult> CreateUserAsync(User user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task<IdentityResult> AddUserToRoleAsync(User user, string role) =>
            await _userManager.AddToRoleAsync(user, role);

        public async Task<IdentityResult> UpdateUserAsync(User user) =>
            await _userManager.UpdateAsync(user);

        public async Task<IdentityResult> DeleteUserAsync(User user) =>
            await _userManager.DeleteAsync(user);

        public async Task<List<UserRole>> GetRolesFromRoleManagerAsync() => await _roleManager.Roles.ToListAsync();
        public async Task<IdentityResult> CreateRoleAsync(UserRole role) => await _roleManager.CreateAsync(role);

        public async Task<IdentityResult> DeleteRoleAsync(UserRole role) => await _roleManager.DeleteAsync(role);

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure)
            => await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

        public async Task SignOutAsync() => await _signInManager.SignOutAsync();


        public UserManager<User> GetUserManager() => _userManager;
        public RoleManager<UserRole> GetRoleManager() => _roleManager;
        public SignInManager<User> GetSignInManager() => _signInManager;
    }
}
