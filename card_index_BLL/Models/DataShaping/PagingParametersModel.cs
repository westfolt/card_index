namespace card_index_BLL.Models.DataShaping
{
    /// <summary>
    /// Model with paging properties
    /// </summary>
    public class PagingParametersModel
    {
        const int maxPageSize = 20;
        private int _pageSize = 2;

        /// <summary>
        /// Number of entities, showing on screen
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        /// <summary>
        /// Number of page
        /// </summary>
        public int PageNumber { get; set; } = 1;
    }
}
