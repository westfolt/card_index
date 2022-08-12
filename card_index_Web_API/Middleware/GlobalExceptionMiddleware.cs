using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace card_index_Web_API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CardIndexException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non-awaited error occured:\n{ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new Response(false, $"Non-awaited error occured: {ex.Message}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var converted = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(converted);
        }
    }
}
