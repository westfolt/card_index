using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using card_index_BLL.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace card_index_Web_API.Filters
{
    /// <summary>
    /// Filter for handling model validation logic
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        private readonly List<Type> _acceptedTypes = new List<Type>()
        {
            typeof(BaseDto),
            typeof(UserInfoModel),
            typeof(ChangePasswordModel),
            typeof(UserRoleInfoModel),
            typeof(UserRegistrationModel),
            typeof(UserLoginModel)
        };

        /// <summary>
        /// Action performed on controller action executing,
        /// performs validation of given model and returns result
        /// depending on valid or not
        /// </summary>
        /// <param name="context">Context of controller action</param>
        /// <exception cref="NullReferenceException">Raised, when cannot find appropriate validator for given entity</exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var entity = GetModelValue(context.ActionArguments);
            if (entity == null)
            {
                context.Result = new BadRequestObjectResult(new Response(false, "No model passed"));
                return;
            }

            IValidator validator = null;
            IValidationContext validationContext = null;

            switch (entity)
            {
                case AuthorDto dto:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<AuthorDto>>();
                    validationContext = new ValidationContext<AuthorDto>(dto);
                    break;
                case GenreDto dto:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<GenreDto>>();
                    validationContext = new ValidationContext<GenreDto>(dto);
                    break;
                case RateDetailDto dto:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<RateDetailDto>>();
                    validationContext = new ValidationContext<RateDetailDto>(dto);
                    break;
                case TextCardDto dto:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<TextCardDto>>();
                    validationContext = new ValidationContext<TextCardDto>(dto);
                    break;
                case UserInfoModel model:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<UserInfoModel>>();
                    validationContext = new ValidationContext<UserInfoModel>(model);
                    break;
                case UserRoleInfoModel model:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<UserRoleInfoModel>>();
                    validationContext = new ValidationContext<UserRoleInfoModel>(model);
                    break;
                case UserLoginModel model:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<UserLoginModel>>();
                    validationContext = new ValidationContext<UserLoginModel>(model);
                    break;
                case UserRegistrationModel model:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<UserRegistrationModel>>();
                    validationContext = new ValidationContext<UserRegistrationModel>(model);
                    break;
                case ChangePasswordModel model:
                    validator = context.HttpContext.RequestServices.GetService<IValidator<ChangePasswordModel>>();
                    validationContext = new ValidationContext<ChangePasswordModel>(model);
                    break;
            }

            if(validator == null || validationContext == null)
                throw new NullReferenceException("Validator not found for given type");

            var result = validator.Validate(validationContext);

            ProcessValidationResult(context, result, entity);
        }
        /// <summary>
        /// Action performed when controller action executed
        /// </summary>
        /// <param name="context">Context of controller action</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private void ProcessValidationResult(ActionExecutingContext context, ValidationResult result, object entityToValidate)
        {
            if (!result.IsValid)
            {
                var response = new Response()
                {
                    Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
                };

                switch (entityToValidate)
                {
                    case UserLoginModel _:
                        context.Result = new UnauthorizedObjectResult(response);
                        break;
                    default:
                        context.Result = new BadRequestObjectResult(response);
                        break;
                }
            }
        }
        /// <summary>
        /// Helper method, gets entity from context
        /// </summary>
        /// <param name="actionArguments">dictionary of controller action arguments</param>
        /// <returns>Entity object for validation</returns>
        private object GetModelValue(IDictionary<string, object> actionArguments)
        {
            foreach (var actionArgument in actionArguments)
            {
                if (_acceptedTypes.SingleOrDefault(type => type.IsInstanceOfType(actionArgument.Value)) != null)
                {
                    return actionArgument.Value;
                }
            }

            return null;
        }
    }
}
