using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace card_index_BLL.Validation
{
    public class BirthDateAttribute : ValidationAttribute
    {
        private readonly DateTime _startingDate;
        private readonly DateTime _endDate;
        private readonly int _minYears;
        private readonly int _maxYears;

        public BirthDateAttribute(int minYearsAllowed, int maxYearsAllowed)
        {
            _startingDate = DateTime.Today.AddYears(-1 * maxYearsAllowed);
            _endDate = DateTime.Today.AddYears(-1 * minYearsAllowed);
            _minYears = minYearsAllowed;
            _maxYears = maxYearsAllowed;
        }

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
