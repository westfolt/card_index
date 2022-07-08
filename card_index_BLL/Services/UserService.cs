using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using card_index_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    /// <summary>
    /// Implements interface between webapi and repository
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IManageUsersRoles _usersRolesManager;

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="mapper">Object for entities mapping</param>
        /// <param name="usersRolesManager">Wrapping object for userManager, roleManager and signInManager</param>
        public UserService(IMapper mapper, IManageUsersRoles usersRolesManager)
        {
            _mapper = mapper;
            _usersRolesManager = usersRolesManager;
        }

        /// <summary>
        /// Gets all users info from db and maps to info model
        /// </summary>
        /// <returns>user info list</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<IEnumerable<UserInfoModel>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _usersRolesManager.GetUsersUMAsync();
                var mapped = new List<UserInfoModel>(takenFromDb.Count);
                for (int i = 0; i < takenFromDb.Count; i++)
                {
                    mapped.Add(_mapper.Map<User, UserInfoModel>(takenFromDb[i]));
                    mapped[i].UserRoles = await _usersRolesManager.GetRolesFromUserManagerAsync(takenFromDb[i]);
                }

                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get users from db", ex);
            }
        }

        /// <summary>
        /// Gets user info for user with given id
        /// </summary>
        /// <param name="id">Id of user to search</param>
        /// <returns>Model, mapped from user</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<UserInfoModel> GetByIdAsync(int id)
        {
            try
            {
                var takenFromDb = (await _usersRolesManager.GetUsersUMAsync()).FirstOrDefault(u => u.Id == id);
                var mapped = _mapper.Map<User, UserInfoModel>(takenFromDb);
                if(mapped != null)
                    mapped.UserRoles = await _usersRolesManager.GetRolesFromUserManagerAsync(takenFromDb);

                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get user with id {id} from db", ex);
            }
        }

        /// <summary>
        /// Gets user with given email
        /// </summary>
        /// <param name="email">Email address to search</param>
        /// <returns>User matching criteria</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<UserInfoModel> GetByEmailAsync(string email)
        {
            try
            {
                var takenFromDb = await _usersRolesManager.FindByEmailAsync(email);
                var mapped = _mapper.Map<User, UserInfoModel>(takenFromDb);
                mapped.UserRoles = await _usersRolesManager.GetRolesFromUserManagerAsync(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get user with email {email} from db", ex);
            }
        }

        /// <summary>
        /// Modifies user with given data,
        /// modifies only user data, not identity security values
        /// </summary>
        /// <param name="model">Model containing new user data</param>
        /// <returns>Response object, containing data about operation</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<Response> ModifyUserAsync(UserInfoModel model)
        {
            try
            {
                var takenFromDb = (await _usersRolesManager.GetUsersUMAsync()).FirstOrDefault(u => u.Id == model.Id);
                var userExistingRoles = await _usersRolesManager.GetRolesFromUserManagerAsync(takenFromDb);
                var givenRole = model.UserRoles.ToList()[0];

                //deleting old roles and changing to new
                if (!userExistingRoles.Contains(givenRole))
                {
                    await _usersRolesManager.RemoveFromRolesAsync(takenFromDb, userExistingRoles);
                    await _usersRolesManager.AddUserToRoleAsync(takenFromDb, givenRole);
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

                var result = await _usersRolesManager.UpdateUserAsync(takenFromDb);

                if (result.Succeeded)
                    return new Response(true, $"User {model.FirstName} {model.LastName} successfully modified");

                return new Response() { Errors = result.Errors.Select(e => e.Description).ToList() };

            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot modify user with id: {model.Id}", ex);
            }
        }

        /// <summary>
        /// Deletes user with given id
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        /// <returns>Response info about operation</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<Response> DeleteUserAsync(int id)
        {
            try
            {
                var userToDelete = (await _usersRolesManager.GetUsersUMAsync()).FirstOrDefault(u => u.Id == id);
                if (userToDelete == null)
                    return new Response(false, "No such user in database");

                var result = await _usersRolesManager.DeleteUserAsync(userToDelete);

                if (result.Succeeded)
                    return new Response(true, $"User with id: {id} successfully deleted");

                return new Response() { Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete user with id: {id}", ex);
            }
        }

        /// <summary>
        /// Gets all user roles from DB
        /// </summary>
        /// <returns>List of existing user roles</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<IEnumerable<UserRoleInfoModel>> GetAllRolesAsync()
        {
            try
            {
                var takenFromDb = await _usersRolesManager.GetRolesFromRoleManagerAsync();
                return _mapper.Map<IEnumerable<UserRole>, IEnumerable<UserRoleInfoModel>>(takenFromDb);
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get all roles from db", ex);
            }
        }

        /// <summary>
        /// Gets role with given name from DB
        /// </summary>
        /// <param name="roleName">Role name to search</param>
        /// <returns>Role matching criteria</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<UserRoleInfoModel> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var takenFromDb = (await _usersRolesManager.GetRolesFromRoleManagerAsync()).FirstOrDefault(r => r.Name == roleName);
                return _mapper.Map<UserRole, UserRoleInfoModel>(takenFromDb);

            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get role with email {roleName} from db", ex);
            }
        }

        /// <summary>
        /// Adds new role to DB
        /// </summary>
        /// <param name="model">model, containing data to add</param>
        /// <returns>Id of added role</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<int> AddRoleAsync(UserRoleInfoModel model)
        {
            try
            {
                var mapped = _mapper.Map<UserRoleInfoModel, UserRole>(model);
                mapped.Id = 0;
                await _usersRolesManager.CreateRoleAsync(mapped);
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot add role: {model.RoleName}", ex);
            }
        }

        /// <summary>
        /// Deletes role from DB
        /// </summary>
        /// <param name="roleName">Role name to delete</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task DeleteRoleAsync(string roleName)
        {
            try
            {
                var roleToDelete = (await _usersRolesManager.GetRolesFromRoleManagerAsync()).FirstOrDefault(r => r.Name == roleName);
                await _usersRolesManager.DeleteRoleAsync(roleToDelete);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete role: {roleName}", ex);
            }
        }

        /// <summary>
        /// Changes users password
        /// </summary>
        /// <param name="user">User, who's password you want to change</param>
        /// <param name="currentPassword">current user's password</param>
        /// <param name="newPassword">new user's password</param>
        /// <returns>Response info about operation</returns>
        public async Task<Response> ChangeUserPasswordAsync(UserInfoModel user, string currentPassword, string newPassword)
        {
            try
            {
                var userInDb = await _usersRolesManager.FindByNameAsync(user.Email);
                var result = await _usersRolesManager.ChangeUserPasswordAsync(userInDb, currentPassword, newPassword);
                if(result.Succeeded)
                    return new Response(true, $"Password for user {user.FirstName} {user.LastName} successfully changed!");

                return new Response(false, "Password has not been changed")
                    { Errors = result.Errors.Select(e => e.Description).ToList() };
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Error while changing pass for user with id: {user.Id}", ex);
            }
        }

        /// <summary>
        /// Checks, if given password is valid for given user
        /// </summary>
        /// <param name="user">The user whose password should be validated</param>
        /// <param name="password">The password to validate</param>
        /// <returns>True, if password valid for this user, false otherwise</returns>
        public async Task<bool> CheckPasswordAsync(UserInfoModel user, string password)
        {
            try
            {
                var userInDb = await _usersRolesManager.FindByNameAsync(user.Email);
                return await _usersRolesManager.CheckPasswordAsync(userInDb, password);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot perform password check for userId: {user.Id}", ex);
            }
        }
    }
}
