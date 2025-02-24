using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AncosBarber.Filters.Exceptions;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ApiException apiException)
        {
            context.Result = new ObjectResult(new
            {
                error = apiException.Message
            })
            {
                StatusCode = apiException.StatusCode
            };

            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = new ObjectResult(new
            {
                error = "Ocorreu um error não tratado. Contate-nos!",
                details = context.Exception.Message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}