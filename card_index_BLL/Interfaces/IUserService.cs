using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;

namespace card_index_BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoModel>> GetAllAsync();
        Task<UserInfoModel> GetByIdAsync(int id);
        Task<LoginResponse> LoginUser(UserLoginModel model);
        Task<int> RegisterUser(UserRegistrationModel model);
        Task<Response<string>> ModifyUser(UserModifyModel model);
        Task AddRoleToUser(int userId, string newRole);
        Task RemoveRoleFromUser(int userId, string roleToRemove);
        Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync();
        Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName);
        Task<int> AddRoleAsync(UserRoleInfoModel model);
        Task DeleteRoleAsync(string roleName);
    }
}
