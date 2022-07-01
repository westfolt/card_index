using System.Collections.Generic;

namespace card_index_DAL.Entities
{
    /// <summary>
    /// DEscribes author of text material
    /// </summary>
    public class Author : BaseEntity
    {
        /// <summary>
        /// Author's first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Author's last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Author's year of birth
        /// </summary>
        public int YearOfBirth { get; set; }
        /// <summary>
        /// Collection of text cards, made by this author
        /// </summary>
        public ICollection<TextCard> TextCards { get; set; }
    }
}
