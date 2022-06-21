using System;
using System.ComponentModel.DataAnnotations;

namespace card_index_BLL.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _startingDate;
        private readonly string _userErrorMessage;

        public DateRangeAttribute(string startingDate, string errorMessage)
        {
            _startingDate = DateTime.Parse(startingDate);
            _userErrorMessage = errorMessage;
        }
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
