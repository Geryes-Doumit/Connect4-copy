using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Connect4.Domain.Exceptions;
using Connect4.Common.Model;

namespace Connect4.API;

/// <summary>
/// Global exception filter that transforms custom domain exceptions into appropriate HTTP status codes.
/// </summary>
/// <remarks>
/// <para>
/// - <see cref="UnauthorizedDomainException"/> => 401 (Unauthorized). <br/>
/// - <see cref="BusyUserDomainException"/> => 409 (Conflict). <br/>
/// - <see cref="GameNotFoundException"/> => 404 (Not Found). <br/>
/// - <see cref="NotYourTurnDomainException"/> => 403 (Forbidden). <br/>
/// - <see cref="DomainException"/> => 400 (Bad Request). <br/>
/// - Otherwise => 500 (Internal Server Error).
/// </para>
/// </remarks>
public class HttpResponseExceptionFilter : IExceptionFilter
{
    /// <summary>
    /// Invoked when an exception is thrown during controller action execution.
    /// Maps specific exceptions to corresponding HTTP responses.
    /// </summary>
    /// <param name="context">Contains the exception and context data.</param>
    public void OnException(ExceptionContext context)
    {
        // UnauthorizedDomainException => 401
        if (context.Exception is UnauthorizedDomainException exAuth)
        {
            context.Result = new JsonResult(new MessageResponsDto { Message = exAuth.Message })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            context.ExceptionHandled = true;
            return;
        }

        // BusyUserDomainException => 409
        if (context.Exception is BusyUserDomainException exBusy)
        {
            context.Result = new JsonResult(new GameIdResponsDto { Message = exBusy.Message, GameId = exBusy.GameId })
            {
                StatusCode = StatusCodes.Status409Conflict
            };
            context.ExceptionHandled = true;
            return;
        }

        // GameNotFoundException => 404
        if (context.Exception is GameNotFoundException exGameNotFound)
        {
            context.Result = new JsonResult(new MessageResponsDto { Message = exGameNotFound.Message })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
            context.ExceptionHandled = true;
            return;
        }

        // NotYourTurnDomainException => 403
        if (context.Exception is NotYourTurnDomainException exNotYourTurn)
        {
            context.Result = new JsonResult(new MessageResponsDto { Message = exNotYourTurn.Message })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            context.ExceptionHandled = true;
            return;
        }

        // DomainException => 400
        if (context.Exception is DomainException exDomain)
        {
            context.Result = new JsonResult(new MessageResponsDto { Message = exDomain.Message })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            context.ExceptionHandled = true;
            return;
        }

        // Default => 500
        context.Result = new JsonResult(new MessageResponsDto { Message = "An error occurred" })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}
