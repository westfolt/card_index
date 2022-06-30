using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_BLL.Models.DataShaping
{
    public class PagingParametersModel
    {
        const int maxPageSize = 20;
        private int _pageSize = 2;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public int PageNumber { get; set; } = 1;
    }
}
