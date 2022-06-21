using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public ICollection<RateDetail> RateDetails { get; set; }
    }
}
