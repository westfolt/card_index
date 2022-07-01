using System;
using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    /// <summary>
    /// Describes card for text material
    /// </summary>
    public class TextCard : BaseEntity
    {
        /// <summary>
        /// Name of text card
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Date, when text has been released
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Rating of card, is made from users's marks
        /// </summary>
        public double CardRating { get; set; }
        /// <summary>
        /// Id of genre, to which text belongs
        /// </summary>
        public int GenreId { get; set; }
        /// <summary>
        /// Genre entity, to which text belongs
        /// </summary>
        public Genre Genre { get; set; }
        /// <summary>
        /// Collection of authors for this text
        /// </summary>
        public ICollection<Author> Authors { get; set; }
        /// <summary>
        /// Collection of user-given marks for text material,
        /// is connection between textCard and user
        /// </summary>
        public ICollection<RateDetail> RateDetails { get; set; }
    }
}
