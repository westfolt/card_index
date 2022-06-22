namespace card_index_BLL.Models
{
    /// <summary>
    /// Filtering model for card objects filter
    /// </summary>
    public class FilterModel
    {
        /// <summary>
        /// Author identifier
        /// </summary>
        public int? AuthorId { get; set; }
        /// <summary>
        /// Genre identifier
        /// </summary>
        public int? GenreId { get; set; }
        /// <summary>
        /// Text card rating
        /// </summary>
        public double? Rating { get; set; }
    }
}
