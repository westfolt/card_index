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
        /// Author first name, required, should have 3+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Firstname is empty")]
        [RegularExpression(@"[\w,.\-']{3,}", ErrorMessage = "More than 3 characters, alphanumeric")]
        public string FirstName { get; set; }
        /// <summary>
        /// Author last name, required, should have 4+ characters, cannot contain numbers
        /// </summary>
        [Required(ErrorMessage = "Lastname is empty")]
        [RegularExpression(@"[\w,.\-']{4,}", ErrorMessage = "More than 4 characters, alphanumeric")]
        public string LastName { get; set; }
        /// <summary>
        /// Author birth year, values from 1900 to 2004 allowed
        /// </summary>
        [Range(1900, 2004, ErrorMessage = "Birth year from 1900 to 2004 allowed")]
        public int YearOfBirth { get; set; }
        /// <summary>
        /// Collection of text card ids
        /// </summary>
        public ICollection<int> TextCardIds { get; set; }
    }
}
