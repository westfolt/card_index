using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes author
    /// </summary>
    public class AuthorDto : BaseDto
    {
        /// <summary>
        /// Author first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Author last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Author birth year
        /// </summary>
        public int YearOfBirth { get; set; }
        /// <summary>
        /// Collection of text card ids
        /// </summary>
        public ICollection<int> TextCardIds { get; set; }
    }
}
