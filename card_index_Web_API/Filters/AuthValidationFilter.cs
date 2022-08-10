using System.Linq;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace card_index_Web_API.Filters
{
    public class AuthValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var entity = context.ActionArguments.SingleOrDefault(e => e.Value is UserRegistrationModel || e.Value is UserLoginModel);
            Response response = null;
            if (entity.Value == null)
            {
                context.Result = new BadRequestObjectResult(new Response(false, "No model passed"));
            }

            if (!context.ModelState.IsValid)
            {
                response = new Response()
                {
                    Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                        .ToList()
                };
            
                switch (entity.Value)
                {
                    case UserLoginModel userLogin:
                        context.Result = new UnauthorizedObjectResult(response);
                        break;
                    case UserRegistrationModel userRegistration:
                        context.Result = new BadRequestObjectResult(response);
                        break;
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
