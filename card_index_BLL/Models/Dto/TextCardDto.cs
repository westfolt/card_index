using card_index_BLL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes text card
    /// </summary>
    public class TextCardDto
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Card name, required, should have 6+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Title is empty")]
        [MinLength(6, ErrorMessage = "Should be longer than 6 characters")]
        public string Title { get; set; }
        /// <summary>
        /// Card release date: from 1/1/1900 till today
        /// </summary>
        [DateRange("1/1/1900", "Release date from 1/1/1900 till today")]
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Card rating value
        /// </summary>
        public double CardRating { get; set; }
        /// <summary>
        /// Collection of author ids
        /// </summary>
        public ICollection<int> AuthorIds { get; set; }
        /// <summary>
        /// Collection of rating details
        /// </summary>
        public ICollection<int> RateDetailsIds { get; set; }
        /// <summary>
        /// Genre name, required, should have 3+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Genre is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, alphanumeric")]
        public string GenreName { get; set; }
    }
}
