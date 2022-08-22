using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Identity.Models
{
    /// <summary>
    /// Model for user registration
    /// </summary>
    public class UserRegistrationModel
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User phone number in format +X(XXX)XXXXXXX
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Confirm password field
        /// </summary>
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
