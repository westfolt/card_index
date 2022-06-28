using System;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Custom age validation attribute
    /// </summary>
    public class BirthDateAttribute : ValidationAttribute
    {
        private readonly DateTime _startingDate;
        private readonly DateTime _endDate;
        private readonly int _minYears;
        private readonly int _maxYears;

        /// <summary>
        /// Takes age range as parameters
        /// </summary>
        /// <param name="minYearsAllowed">Min allowed age</param>
        /// <param name="maxYearsAllowed">Max allowed age</param>
        public BirthDateAttribute(int minYearsAllowed, int maxYearsAllowed)
        {
            _startingDate = DateTime.Today.AddYears(-1 * maxYearsAllowed);
            _endDate = DateTime.Today.AddYears(-1 * minYearsAllowed);
            _minYears = minYearsAllowed;
            _maxYears = maxYearsAllowed;
        }

        /// <summary>
        /// Validation method
        /// </summary>
        /// <param name="value">age value to validate</param>
        /// <returns>Validation result</returns>
        public override bool IsValid(object value)
        {
            if (value is DateTime userInput)
            {
                if (userInput >= _startingDate && userInput <= _endDate)
                {
                    return true;
                }

                ErrorMessage = $"Age should be between {_minYears} and {_maxYears} years";
                return false;
            }

            ErrorMessage = "Not DateTime type";
            return false;
        }
    }
}
