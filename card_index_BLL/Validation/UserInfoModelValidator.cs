using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Identity.Models;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for UserInfoModel entity
    /// </summary>
    public class UserInfoModelValidator : AbstractValidator<UserInfoModel>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public UserInfoModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, no numbers");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is empty")
                .Matches(@"[\w,.\-']{4,}").WithMessage("More than 4 characters, no numbers");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Age is required")
                .InclusiveBetween(DateTime.Today.AddYears(-100), DateTime.Today.AddYears(-18))
                .WithMessage("Age should be between 18 and 100 years");
            RuleFor(x => x.City).Matches(@"[\w,.\-']{3,}")
                .WithMessage("More than 3 characters, no numbers");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is mandatory")
                .EmailAddress().WithMessage("Email not valid");
            RuleFor(x => x.Phone).Matches(@"\+[\d]{1,3}\([\d]{3}\)[\d]{7}")
                .WithMessage("Phone should be +X(XXX)XXXXXXX");
        }
    }
}
