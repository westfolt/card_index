using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace card_index_BLL.Models.Identity.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Field is mandatory")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field is mandatory")]
        public string Password { get; set; }
    }
}
