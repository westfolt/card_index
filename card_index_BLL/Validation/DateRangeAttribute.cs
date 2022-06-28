using System;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Custom attribute for date range validation
    /// </summary>
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _startingDate;
        private readonly string _userErrorMessage;

        /// <summary>
        /// Constructor, takes date range
        /// </summary>
        /// <param name="startingDate">starting date</param>
        /// <param name="errorMessage">end of date range</param>
        public DateRangeAttribute(string startingDate, string errorMessage)
        {
            _startingDate = DateTime.Parse(startingDate);
            _userErrorMessage = errorMessage;
        }
        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">date for validation</param>
        /// <returns>validation result</returns>
        public override bool IsValid(object value)
        {
            if (value is DateTime userInput)
            {
                if (userInput >= _startingDate && userInput <= DateTime.Today)
                {
                    return true;
                }

                ErrorMessage = _userErrorMessage;
                return false;
            }

            ErrorMessage = "Not DateTime type";
            return false;
        }
    }
}
