using System;
using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    public class TextCard : BaseEntity
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double CardRating { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<RateDetail> RateDetails { get; set; }
    }
}
