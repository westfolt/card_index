using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_DAL.Entities.DataShaping
{
    public class PagingParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
