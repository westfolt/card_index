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
        /// User role name, required, should have 3+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Role name is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string RoleName { get; set; }
    }
}
