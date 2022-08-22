using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Dto;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for TextCardDto entity
    /// </summary>
    public class TextCardDtoValidator:AbstractValidator<TextCardDto>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public TextCardDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is empty")
                .MinimumLength(6).WithMessage("Should be longer than 6 characters");
            RuleFor(x => x.ReleaseDate).InclusiveBetween(new DateTime(1900, 1, 1), DateTime.Today)
                .WithMessage("Release date from 1/1/1900 till today");
            RuleFor(x => x.GenreName).NotEmpty().WithMessage("Genre is empty")
                .Matches(@"[\w,.\-']{3,}").WithMessage("More than 3 characters, alphanumeric");
        }
    }
}
