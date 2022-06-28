using card_index_BLL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace card_index_BLL.Models.Identity.Models
{
    /// <summary>
    /// User information model
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User firstname
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
        /// User date of birth, allowed age from 18 to 100 years
        /// </summary>
        [Required(ErrorMessage = "Age is required")]
        [BirthDate(18, 100)]
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// User city
        /// </summary>
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string City { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        [Required(ErrorMessage = "Email is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        /// <summary>
        /// User phone number in format +X(XXX)XXXXXXX
        /// </summary>
        [RegularExpression(@"\+[\d]{1,3}\([\d]{3}\)[\d]{7}",
            ErrorMessage = "Phone should be +X(XXX)XXXXXXX")]
        public string Phone { get; set; }
        /// <summary>
        /// Collection of user roles
        /// </summary>
        [NotMapped]
        public ICollection<string> UserRoles { get; set; }
    }
}
