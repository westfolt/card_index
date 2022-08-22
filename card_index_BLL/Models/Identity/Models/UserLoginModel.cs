using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Identity.Models
{
    /// <summary>
    /// Model for user login
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// User email (same as username)
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
    }
}
