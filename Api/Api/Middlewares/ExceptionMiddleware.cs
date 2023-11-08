using Api.Exceptions;
using Api.Models.Dtos;
using Api.Models.Enums;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Api.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);

                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            ErrorDto errorDto = null;

            switch(e)
            {
                case AddressNotFoundException:
                    context.Response.StatusCode = 404;
                    errorDto = new ErrorDto(ErrorType.NotFound);
                    break;
                default:
                    context.Response.StatusCode = 500;
                    errorDto = new ErrorDto(ErrorType.Unexcepted);
                    break;
            }

            await context.Response.WriteAsJsonAsync(errorDto);
        }
    }
}
