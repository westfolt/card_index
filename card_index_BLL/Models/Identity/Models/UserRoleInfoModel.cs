using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Identity.Models
{
    public class UserRoleInfoModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Role name is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string RoleName { get; set; }
    }
}
