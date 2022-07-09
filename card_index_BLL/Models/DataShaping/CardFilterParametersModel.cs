namespace card_index_BLL.Models.DataShaping
{
    /// <summary>
    /// Filtering model for card objects filtering
    /// </summary>
    public class CardFilterParametersModel : PagingParametersModel
    {
        private const int maxRate = 5;
        private const int minRate = 0;
        private int _rating = 0;

        /// <summary>
        /// Name of text card
        /// </summary>
        public string CardName { get; set; } = "";

        /// <summary>
        /// Author identifier
        /// </summary>
        public int AuthorId { get; set; } = 0;

        /// <summary>
        /// Genre identifier
        /// </summary>
        public int GenreId { get; set; } = 0;

        /// <summary>
        /// Text card rating
        /// </summary>
        public int Rating
        {
            get => _rating;
            set
            {
                if (value < minRate)
                    _rating = minRate;
                else if (value > maxRate)
                    _rating = maxRate;
                else
                    _rating = value;
            }
        }
    }
}
