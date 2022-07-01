namespace card_index_DAL.Entities
{
    /// <summary>
    /// Details of mark, given by user to particular text,
    /// connects textCard and user in DB
    /// </summary>
    public class RateDetail : BaseEntity
    {
        /// <summary>
        /// Id of user, who gives rating
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User, who gives rating
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Text card identifier
        /// </summary>
        public int TextCardId { get; set; }
        /// <summary>
        /// Text card, to which rating is given
        /// </summary>
        public TextCard TextCard { get; set; }
        /// <summary>
        /// Given rating value
        /// </summary>
        public ushort RateValue { get; set; }
    }
}
