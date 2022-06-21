using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<TextCard> TextCards { get; set; }
    }
}
