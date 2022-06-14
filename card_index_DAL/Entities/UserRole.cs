using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace card_index_DAL.Entities
{
    public class UserRole:IdentityRole<int>
    {
        public string RoleDescription { get; set; }
    }
}
