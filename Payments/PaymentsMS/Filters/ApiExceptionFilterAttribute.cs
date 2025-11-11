
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PaymentsMS.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ArgumentNullException), HandleArgumentNullException },
                { typeof(ArgumentException), HandleArgumentException },
                { typeof(Exception), HandleException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleArgumentNullException(ExceptionContext context)
        {
            var exception = (ArgumentNullException)context.Exception;
            _logger.LogError(exception, "Un ArgumentNullException ha ocurrido.");
            var details = new ProblemDetails
            {
                Status = 400,
                Title = "Un ArgumentNullException ha ocurrido mientras se procesaba la solicitud.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message
            };
            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleArgumentException(ExceptionContext context)
        {
            var exception = (ArgumentException)context.Exception;
            _logger.LogError(exception, "Un ArgumentException ha ocurrido.");
            var details = new ProblemDetails
            {
                Status = 400,
                Title = "Un ArgumentException ha ocurrido mientras se procesaba la solicitud.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message
            };
            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Un error inesperado ha ocurrido.");
            var details = new ProblemDetails
            {
                Status = 500,
                Title = "Un error inesperado ha ocurrido.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = "Un error inesperado ha ocurrido. Por favor, intente de nuevo más tarde."
            };
            context.Result = new ObjectResult(details)
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
