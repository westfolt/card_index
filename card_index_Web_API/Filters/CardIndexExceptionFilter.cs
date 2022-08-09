using System.Net;
using System.Net.Http;
using card_index_BLL.Exceptions;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace card_index_Web_API.Filters
{
    public class CardIndexExceptionFilter:IExceptionFilter
    {
        private readonly ILogger<CardIndexExceptionFilter> _logger;
        public CardIndexExceptionFilter(ILogger<CardIndexExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var exType = context.Exception.GetType();
            if (exType == typeof(CardIndexException))
            {
                _logger.LogWarning(context.ActionDescriptor.DisplayName + "\n" + context.Exception.Message);
                context.Result = new BadRequestObjectResult(new Response(false, context.Exception.Message));
            }
        }
    }
}
