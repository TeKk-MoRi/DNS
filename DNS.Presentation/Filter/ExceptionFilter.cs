using DNS.Application.Common.Exceptions;
using DNS.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DNS.Presentation.Filter;

public class CatchExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public CatchExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
{
    { typeof(ValidationException), HandleValidationException },
    { typeof(NotFoundException), HandleNotFoundException },
    { typeof(DomainException), HandleDomainException }
};

    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext>? value))
        {
            value.Invoke(context);
            return;
        }
    }


    private void HandleDomainException(ExceptionContext context)
    {
        var exception = (DomainException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "Domain rule violation",
            Detail = exception.Message,
            Status = StatusCodes.Status400BadRequest
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }


    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Title = "Resource Not Found",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }


    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = string.Empty
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }
}

