using Microsoft.AspNetCore.Identity;

namespace card_index_DAL.Entities
{
    /// <summary>
    /// Describes user role in system, other
    /// properties are taken from IdentityRole,
    /// typed with integer for identity
    /// </summary>
    public class UserRole : IdentityRole<int>
    {
        /// <summary>
        /// Description of current role in system
        /// </summary>
        public string RoleDescription { get; set; }
    }
}
