using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_BLL.Models
{
    public class FilterModel
    {
        public int? AuthorId { get; set; }
        public int? GenreId { get; set; }
        public double? Rating { get; set; }
    }
}
