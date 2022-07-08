using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;

namespace CardIndexTests.WebApiTests.Helpers
{
    public class FakeUserService:IUserService
    {
        public Task<IEnumerable<UserInfoModel>> GetAllAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<UserInfoModel> GetByIdAsync(int id)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<UserInfoModel> GetByEmailAsync(string email)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<Response> ModifyUserAsync(UserInfoModel model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<Response> DeleteUserAsync(int id)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync()
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<int> AddRoleAsync(UserRoleInfoModel model)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task DeleteRoleAsync(string roleName)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<Response> ChangeUserPasswordAsync(UserInfoModel user, string currentPassword, string newPassword)
        {
            throw new CardIndexException("Something went wrong");
        }

        public Task<bool> CheckPasswordAsync(UserInfoModel user, string password)
        {
            throw new CardIndexException("Something went wrong");
        }
    }
}
