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
        [Required(ErrorMessage = "Field is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "Field is mandatory")]
        public string Password { get; set; }
    }
}
