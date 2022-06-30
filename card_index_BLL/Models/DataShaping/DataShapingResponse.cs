using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_BLL.Models.DataShaping
{
    /// <summary>
    /// For returning portions of data to user
    /// </summary>
    /// <typeparam name="T">One of object Dto models</typeparam>
    public class DataShapingResponse<T> where T: class
    {
        /// <summary>
        /// Total number of objects in DB
        /// </summary>
        public int TotalNumber { get; set; }
        /// <summary>
        /// Data given to user
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
