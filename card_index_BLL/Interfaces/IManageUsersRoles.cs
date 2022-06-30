using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace card_index_BLL.Interfaces
{
    public interface IManageUsersRoles
    {
        Task<List<User>> GetUsersUMAsync();
        Task<IList<string>> GetRolesFromUserManagerAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByNameAsync(string userName);
        Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(User user);

        Task<List<UserRole>> GetRolesFromRoleManagerAsync();
        Task<IdentityResult> CreateRoleAsync(UserRole role);
        Task<IdentityResult> DeleteRoleAsync(UserRole role);

        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
            bool lockoutOnFailure);

        Task SignOutAsync();

        UserManager<User> GetUserManager();
        RoleManager<UserRole> GetRoleManager();
        SignInManager<User> GetSignInManager();

    }
}
