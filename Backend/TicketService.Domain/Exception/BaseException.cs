namespace TicketService.Domain.Exception
{
    public abstract class BaseException : SystemException
    {
        public string ErrorCode { get; }
        public string? Id { get; }
        public DateTimeOffset Timestamp { get; }
        public string? UserFriendlyMessage { get; }

        public BaseException(string message, string errorCode, string? id = null, string? userFriendlyMessage = null, SystemException? innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Id = id;
            Timestamp = DateTimeOffset.UtcNow;
            UserFriendlyMessage = userFriendlyMessage;
        }

    }
}