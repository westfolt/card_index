using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace card_index_BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoModel>> GetAllAsync();
        Task<UserInfoModel> GetByIdAsync(int id);
        Task<UserInfoModel> GetByEmailAsync(string email);
        Task<Response> ModifyUserAsync(UserInfoModel model);
        Task<Response> DeleteUserAsync(int id);
        Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync();
        Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName);
        Task<int> AddRoleAsync(UserRoleInfoModel model);
        Task DeleteRoleAsync(string roleName);
    }
}
