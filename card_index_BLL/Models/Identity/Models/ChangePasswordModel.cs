using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Identity.Models
{
    /// <summary>
    /// Wrapping class for change password request
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Current user password
        /// </summary>
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        /// <summary>
        /// Confirm password field
        /// </summary>
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
