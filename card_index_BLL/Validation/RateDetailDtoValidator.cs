using System;
using System.Collections.Generic;
using System.Text;
using card_index_BLL.Models.Dto;
using FluentValidation;

namespace card_index_BLL.Validation
{
    /// <summary>
    /// Validator for RateDetailDto entity
    /// </summary>
    public class RateDetailDtoValidator:AbstractValidator<RateDetailDto>
    {
        /// <summary>
        /// Constructor, contains validation rules
        /// </summary>
        public RateDetailDtoValidator()
        {
            RuleFor(x => x.RateValue).InclusiveBetween((ushort)0, (ushort)5)
                .WithMessage("Rating should be from 0 to 5");
        }
    }
}
