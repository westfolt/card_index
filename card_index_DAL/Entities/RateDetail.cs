using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_DAL.Entities
{
    public class RateDetail:BaseEntity
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int TextCardId { get; set; }
        public TextCard TextCard { get; set; }
        public double RateValue { get; set; }
    }
}
