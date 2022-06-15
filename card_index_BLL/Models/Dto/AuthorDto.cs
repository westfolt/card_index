using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace card_index_BLL.Models.Dto
{
    public class AuthorDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression("[\\w,.-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is empty")]
        [RegularExpression("[\\w,.-']{4,}", ErrorMessage = "More than 4 characters, no numbers")]
        public string LastName { get; set; }
        public ICollection<int> TextCardIds { get; set; }
    }
}
