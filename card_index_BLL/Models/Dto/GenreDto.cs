using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes genre of text card
    /// </summary>
    public class GenreDto : BaseDto
    {
        /// <summary>
        /// Genre name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Collection of text card ids
        /// </summary>
        public ICollection<int> TextCardIds { get; set; }
    }
}
