using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is ArgumentException argumentException)
        {
            problemDetails.Title = "Erro de Validação";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = argumentException.Message;
        }
        else if (exception is KeyNotFoundException keyNotFoundException)
        {
            problemDetails.Title = "Recurso não encontrado";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = keyNotFoundException.Message;
        }
        else
        {
            problemDetails.Title = "Erro interno no servidor";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}