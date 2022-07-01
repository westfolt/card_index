using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    /// <summary>
    /// Describes some user parameters, other are taken
    /// from IdentityUser, identifier typed as integer
    /// </summary>
    public class User : IdentityUser<int>
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
        /// User date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// User city of origin
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Details of marks, given by user
        /// </summary>
        public ICollection<RateDetail> RateDetails { get; set; }
    }
}
