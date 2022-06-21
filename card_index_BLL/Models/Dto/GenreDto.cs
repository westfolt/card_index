using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    public class GenreDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is empty")]
        [RegularExpression(@"[\w,.\-']{6,}", ErrorMessage = "More than 6 characters, no numbers")]
        public string Title { get; set; }
        public ICollection<int> TextCardIds { get; set; }
    }
}
