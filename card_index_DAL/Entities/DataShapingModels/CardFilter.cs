using System;
using System.Collections.Generic;
using System.Text;
using card_index_DAL.Entities.DataShaping;

namespace card_index_DAL.Entities.DataShapingModels
{
    public class CardFilter:PagingParameters
    {
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
        public double Rating { get; set; } = 0;
    }
}
