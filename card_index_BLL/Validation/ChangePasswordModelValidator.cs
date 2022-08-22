using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Identity.Models;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for ChangePasswordModel entity
    /// </summary>
    public class ChangePasswordModelValidator:AbstractValidator<ChangePasswordModel>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public ChangePasswordModelValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword)
                .WithMessage("Passwords doesn't match");
        }
    }
}
