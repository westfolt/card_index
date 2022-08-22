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
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// User city
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User phone number in format +X(XXX)XXXXXXX
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Collection of user roles
        /// </summary>
        [NotMapped]
        public ICollection<string> UserRoles { get; set; }
    }
}
