using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;

namespace card_index_BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoModel>> GetAllAsync();
        Task<UserInfoModel> GetByIdAsync(int id);
        Task<UserInfoModel> GetByEmailAsync(string email);
        Task<Response> ModifyUserAsync(UserInfoModel model);
        Task<Response> DeleteUserAsync(int id);
        Task AddRoleToUserAsync(int userId, string newRole);
        Task RemoveRoleFromUserAsync(int userId, string roleToRemove);
        Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync();
        Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName);
        Task<int> AddRoleAsync(UserRoleInfoModel model);
        Task DeleteRoleAsync(string roleName);
    }
}
