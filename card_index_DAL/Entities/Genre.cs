using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    /// <summary>
    /// Describes genre, to which belong text materials
    /// </summary>
    public class Genre : BaseEntity
    {
        /// <summary>
        /// Genre name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Collection of text cards of this genre
        /// </summary>
        public ICollection<TextCard> TextCards { get; set; }
    }
}
