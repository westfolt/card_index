using card_index_BLL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    public class TextCardDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is empty")]
        [MinLength(6, ErrorMessage = "Should be longer than 6 characters")]
        public string Title { get; set; }
        [DateRange("1/1/1900", "Release date from 1/1/1900 till today")]
        public DateTime ReleaseDate { get; set; }
        public double CardRating { get; set; }
        public ICollection<int> AuthorIds { get; set; }
        public ICollection<int> RateDetailsIds { get; set; }
        [Required(ErrorMessage = "Genre is empty")]
        [RegularExpression(@"[\w,.\-']{6,}", ErrorMessage = "More than 6 characters, no numbers")]
        public string GenreName { get; set; }
    }
}
