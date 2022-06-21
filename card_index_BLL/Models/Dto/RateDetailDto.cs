using System;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    public class RateDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TextCardId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardName { get; set; }
        [Range(0, 5, ErrorMessage = "Rating should be from 0 to 5")]
        public ushort RateValue { get; set; }
    }
}
