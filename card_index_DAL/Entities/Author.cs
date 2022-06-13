using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_DAL.Entities
{
    public class Author:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearOfBirth { get; set; }
        public ICollection<TextCard> TextCards { get; set; }
    }
}
