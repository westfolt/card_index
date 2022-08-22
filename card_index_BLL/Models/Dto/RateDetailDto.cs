using System;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Models.Dto
{
    /// <summary>
    /// Describes rating details, connects user and text card
    /// </summary>
    public class RateDetailDto : BaseDto
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Text card identifier
        /// </summary>
        public int TextCardId { get; set; }
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Title of the text card
        /// </summary>
        public string CardName { get; set; }
        /// <summary>
        /// Text card rating
        /// </summary>
        public ushort RateValue { get; set; }
    }
}
