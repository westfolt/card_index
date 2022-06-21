namespace card_index_DAL.Entities
{
    public class RateDetail : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int TextCardId { get; set; }
        public TextCard TextCard { get; set; }
        public ushort RateValue { get; set; }
    }
}
