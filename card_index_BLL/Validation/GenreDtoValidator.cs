using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Dto;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for GenreDto entity
    /// </summary>
    public class GenreDtoValidator:AbstractValidator<GenreDto>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public GenreDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, alphanumeric");
        }
    }
}
