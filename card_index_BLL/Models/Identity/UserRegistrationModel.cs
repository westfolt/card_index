using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace card_index_BLL.Dto.Identity
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression(@"[\w,.-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is empty")]
        [RegularExpression(@"[\w,.-']{4,}", ErrorMessage = "More than 4 characters, no numbers")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [RegularExpression(@"\+[\d]{1, 3}\([\d]{3}\)[\d] { 7}",
            ErrorMessage = "Phone should be +X(XXX)XXXXXXX")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
