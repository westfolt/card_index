using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using card_index_BLL.Models.Identity.Models;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for UserRegistrationModel entity
    /// </summary>
    public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public UserRegistrationModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, no numbers");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is empty")
                .Matches(@"[\w,.\-']{4,}").WithMessage("More than 4 characters, no numbers");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is mandatory")
                .EmailAddress().WithMessage("Email not valid");
            RuleFor(x => x.Phone).Matches(@"\+[\d]{1, 3}\([\d]{3}\)[\d] { 7}")
                .WithMessage("Phone should be +X(XXX)XXXXXXX");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Passwords doesn't match");
        }
    }
}
