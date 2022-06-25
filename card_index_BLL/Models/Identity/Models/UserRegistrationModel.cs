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
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        [Required(ErrorMessage = "Lastname is empty")]
        [RegularExpression(@"[\w,.\-']{4,}", ErrorMessage = "More than 4 characters, no numbers")]
        public string LastName { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        /// <summary>
        /// User phone number in format +X(XXX)XXXXXXX
        /// </summary>
        [RegularExpression(@"\+[\d]{1, 3}\([\d]{3}\)[\d] { 7}",
            ErrorMessage = "Phone should be +X(XXX)XXXXXXX")]
        public string? Phone { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Confirm password field
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
