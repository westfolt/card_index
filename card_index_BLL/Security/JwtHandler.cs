using card_index_BLL.Interfaces;
using card_index_DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace card_index_BLL.Security
{
    /// <summary>
    /// Handles JWT logic
    /// </summary>
    public class JwtHandler
    {
        private readonly IConfigurationSection _jwtConfiguration;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor, takes app config and object, wrapping userManager, roleManager and signInManager
        /// </summary>
        /// <param name="configuration">application configuration</param>
        /// <param name="usersRolesManager">Object, wrapping userManager, roleManager and signInManager</param>
        public JwtHandler(IConfiguration configuration, IManageUsersRoles usersRolesManager)
        {
            _jwtConfiguration = configuration.GetSection("JwtSettings");
            _userManager = usersRolesManager.GetUserManager();
        }

        /// <summary>
        /// Generates JWT for user
        /// </summary>
        /// <param name="user">User, who logged in</param>
        /// <returns>String containing token</returns>
        public async Task<string> GenerateJwt(User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var options = GetTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(options);

            return token;
        }

        /// <summary>
        /// Gets credentials for digital signing
        /// </summary>
        /// <returns>Signing credentials made from user key</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Gets list of claims for user
        /// </summary>
        /// <param name="user">User to get claims for</param>
        /// <returns>List containing claims</returns>
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        /// <summary>
        /// Gets token options based on digital signature and claims list
        /// </summary>
        /// <param name="signingCredentials">Digital signature</param>
        /// <param name="claims">Claims list</param>
        /// <returns>Token options</returns>
        private JwtSecurityToken GetTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var options = new JwtSecurityToken(
                issuer: _jwtConfiguration["validIssuer"],
                audience: _jwtConfiguration["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration["expiryInMinutes"])),
                signingCredentials: signingCredentials);

            return options;
        }
    }
}
