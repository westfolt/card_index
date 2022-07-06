using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        /// <summary>
        /// Confirm password field
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords doesn't match")]
        public string ConfirmNewPassword { get; set; }
    }
}
