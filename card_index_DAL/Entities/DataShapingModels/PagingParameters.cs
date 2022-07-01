namespace card_index_DAL.Entities.DataShaping
{
    /// <summary>
    /// Entity for paging parameters, transfered to repository from BLL
    /// </summary>
    public class PagingParameters
    {
        /// <summary>
        /// Number of page, selected by user
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Page size selected
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
