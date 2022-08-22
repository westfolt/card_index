using card_index_BLL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes text card
    /// </summary>
    public class TextCardDto : BaseDto
    {
        /// <summary>
        /// Card name
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Card release date
        /// </summary>
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
        /// Genre name
        /// </summary>
        public string GenreName { get; set; }
    }
}
