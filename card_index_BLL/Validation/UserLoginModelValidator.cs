using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Identity.Models;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for UserLoginModel entity
    /// </summary>
    public class UserLoginModelValidator:AbstractValidator<UserLoginModel>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public UserLoginModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Field is mandatory")
                .EmailAddress().WithMessage("Email not valid");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Field is mandatory");
        }
    }
}
