using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Dto;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for AuthorDto entity
    /// </summary>
    public class AuthorDtoValidator:AbstractValidator<AuthorDto>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public AuthorDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, alphanumeric");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is empty")
                .Matches(@"[\w,.\-']{4,}").WithMessage("More than 4 characters, alphanumeric");
            RuleFor(x => x.YearOfBirth).InclusiveBetween(1900, 2004)
                .WithMessage("Birth year from 1900 to 2004 allowed");
        }
    }
}
