using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Identity.Models
{
    /// <summary>
    /// Describes user role
    /// </summary>
    public class UserRoleInfoModel
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// User role name
        /// </summary>
        public string RoleName { get; set; }
    }
}
