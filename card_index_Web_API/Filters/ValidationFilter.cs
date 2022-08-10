using System.Linq;
using card_index_BLL.Exceptions;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace card_index_Web_API.Filters
{
    public class ValidationFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var entity = context.ActionArguments.SingleOrDefault(e => e.Value is BaseDto);
            if (entity.Value == null)
            {
                context.Result = new BadRequestObjectResult(new Response(false, "No model passed"));
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new Response()
                {
                    Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
