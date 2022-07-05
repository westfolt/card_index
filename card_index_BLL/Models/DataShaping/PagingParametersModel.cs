namespace card_index_BLL.Models.DataShaping
{
    /// <summary>
    /// Model with paging properties
    /// </summary>
    public class PagingParametersModel
    {
        const int maxPageSize = 20;
        private const int minPageSize = 2;
        private int _pageSize = 2;

        /// <summary>
        /// Number of entities, showing on screen
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < minPageSize)
                    _pageSize = minPageSize;
                else if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }
        }
        /// <summary>
        /// Number of page
        /// </summary>
        public int PageNumber { get; set; } = 1;
    }
}
