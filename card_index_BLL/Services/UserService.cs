using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;

        public UserService(IMapper mapper, UserManager<User> userManager, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserInfoModel>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _userManager.Users.ToListAsync();
                var mapped = new List<UserInfoModel>(takenFromDb.Count);
                for (int i = 0; i < takenFromDb.Count; i++)
                {
                    mapped.Add(_mapper.Map<User, UserInfoModel>(takenFromDb[i]));
                    mapped[i].UserRoles = await _userManager.GetRolesAsync(takenFromDb[i]);
                }

                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get users from db", ex);
            }
        }

        public async Task<UserInfoModel> GetByIdAsync(int id)
        {
            try
            {
                var takenFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                var mapped = _mapper.Map<User, UserInfoModel>(takenFromDb);
                mapped.UserRoles = await _userManager.GetRolesAsync(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get user with id {id} from db", ex);
            }

        }

        public async Task<UserInfoModel> GetByEmailAsync(string email)
        {
            try
            {
                var takenFromDb = await _userManager.FindByEmailAsync(email);
                var mapped = _mapper.Map<User, UserInfoModel>(takenFromDb);
                mapped.UserRoles = await _userManager.GetRolesAsync(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get user with email {email} from db", ex);
            }
        }

        public async Task<Response> ModifyUserAsync(UserInfoModel model)
        {
            try
            {
                var takenFromDb = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
                var userExistingRoles = await _userManager.GetRolesAsync(takenFromDb);
                var givenRole = model.UserRoles.ToList()[0];

                //deleting old roles and changing to new
                if (!userExistingRoles.Contains(givenRole))
                {
                    await _userManager.RemoveFromRolesAsync(takenFromDb, userExistingRoles);
                    await _userManager.AddToRoleAsync(takenFromDb, givenRole);
                }

                takenFromDb.FirstName = model.FirstName;
                takenFromDb.LastName = model.LastName;
                takenFromDb.DateOfBirth = model.DateOfBirth;
                takenFromDb.City = model.City;
                takenFromDb.Email = model.Email;
                takenFromDb.UserName = model.Email;
                takenFromDb.NormalizedEmail = model.Email.ToUpperInvariant();
                takenFromDb.NormalizedUserName = model.Email.ToUpperInvariant();
                takenFromDb.PhoneNumber = model.Phone;

                var result = await _userManager.UpdateAsync(takenFromDb);

                if (result.Succeeded)
                    return new Response(true, $"User {model.FirstName} {model.LastName} successfully modified");

                return new Response() { Errors = result.Errors.Select(e => e.Description).ToList() };

            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot modify user with id: {model.Id}", ex);
            }
        }

        public async Task<Response> DeleteUserAsync(int id)
        {
            try
            {
                var userToDelete = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (userToDelete == null)
                    return new Response(false, "No such user in database");

                var result = await _userManager.DeleteAsync(userToDelete);

                if (result.Succeeded)
                    return new Response(true, $"User with id: {id} successfully deleted");

                return new Response() { Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete user with id: {id}", ex);
            }
        }

        public async Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync()
        {
            try
            {
                var takenFromDb = await _roleManager.Roles.ToListAsync();
                return _mapper.Map<IEnumerable<UserRole>, IEnumerable<UserRoleInfoModel>>(takenFromDb);
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get all roles from db", ex);
            }
        }

        public async Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var takenFromDb = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                return _mapper.Map<UserRole, UserRoleInfoModel>(takenFromDb);

            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get role with email {roleName} from db", ex);
            }
        }

        public async Task<int> AddRoleAsync(UserRoleInfoModel model)
        {
            try
            {
                var mapped = _mapper.Map<UserRoleInfoModel, UserRole>(model);
                mapped.Id = 0;
                await _roleManager.CreateAsync(mapped);
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot add role: {model.RoleName}", ex);
            }
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            try
            {
                var roleToDelete = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                await _roleManager.DeleteAsync(roleToDelete);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete role: {roleName}", ex);
            }
        }
    }
}
