using Bz.Fott.Registration.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.AccessControl;

namespace Bz.Fott.Registration.WebAPI.ExceptionsHandling;

internal class ErrorObjectResult : ObjectResult
{
    public ErrorObjectResult(object error, int statusCode)
                : base(error)
    {
        StatusCode = statusCode;
    }

    public static ErrorObjectResult Create(Exception exception)
    {
        var message = exception.Message;
        var httpStatusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case ValidationException:
            case FluentValidation.ValidationException:
                httpStatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                httpStatusCode = (int)HttpStatusCode.NotFound;
                break;
            case InvalidOperationException:
                httpStatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var errorResponse = new ErrorResponseDto
        {
            Message = message
        };

        return new ErrorObjectResult(errorResponse, httpStatusCode);
    }
}
