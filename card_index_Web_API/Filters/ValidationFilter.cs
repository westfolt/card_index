using System;
using System.Collections.Generic;
using System.Linq;
using card_index_BLL.Exceptions;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace card_index_Web_API.Filters
{
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
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var entity = GetModelValue(context.ActionArguments);
            if (entity == null)
            {
                context.Result = new BadRequestObjectResult(new Response(false, "No model passed"));
            }

            if (!context.ModelState.IsValid)
            {
                var response = new Response()
                {
                    Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                };

                switch (entity)
                {
                    case UserLoginModel userLogin:
                        context.Result = new UnauthorizedObjectResult(response);
                        break;
                    default:
                        context.Result = new BadRequestObjectResult(response);
                        break;
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

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
