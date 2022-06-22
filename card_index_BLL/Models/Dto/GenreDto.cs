using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes genre of text card
    /// </summary>
    public class GenreDto
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Genre name, required, should have 3+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Title is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, no numbers")]
        public string Title { get; set; }
        /// <summary>
        /// Collection of text card ids
        /// </summary>
        public ICollection<int> TextCardIds { get; set; }
    }
}
