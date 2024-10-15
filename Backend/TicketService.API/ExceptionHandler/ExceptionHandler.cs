using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TicketService.Domain.Exception;

namespace TicketService.API.ExceptionHandler
{
    public class ExceptionHandler : IExceptionHandler
    {
        readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> _logger)
            : base()
        {
            this._logger = _logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            _logger.LogError(
                exception,
                "An exception occurred while processing the request. {Message} ",
                exception.Message
            );

            httpContext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                InternalServerException => (int)HttpStatusCode.InternalServerError,
                BadRequestException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorDetails = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message,
                ErrorCode = (exception as BaseException)?.ErrorCode ?? "INTERNAL SERVER ERROR",
                Id = (exception as BaseException)?.Id ?? Guid.NewGuid().ToString(),
                TrackId = httpContext.TraceIdentifier,
                Timestamp = (exception as BaseException)?.Timestamp ?? DateTimeOffset.UtcNow,
                UserFriendlyMessage = (exception as BaseException)?.UserFriendlyMessage ?? "An error occurred while processing your request.",
                Ressource = httpContext.Request.Method + " " + httpContext.Request.Path
            };
            await httpContext.Response.WriteAsJsonAsync(errorDetails, cancellationToken);

            return true;
        }
    }
}
