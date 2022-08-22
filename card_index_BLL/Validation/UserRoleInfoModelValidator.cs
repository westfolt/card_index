using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Identity.Models;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for UserRoleInfoModel entity
    /// </summary>
    public class UserRoleInfoModelValidator : AbstractValidator<UserRoleInfoModel>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public UserRoleInfoModelValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role name is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, no numbers");
        }
    }
}
