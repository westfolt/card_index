using System.Linq;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_BLL.Models.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace card_index_Web_API.Filters
{
    public class UserValidationFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var entity = context.ActionArguments.SingleOrDefault(e =>
                e.Value is UserInfoModel || e.Value is ChangePasswordModel || e.Value is UserRoleInfoModel);
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
