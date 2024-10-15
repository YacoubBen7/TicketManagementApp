namespace TicketService.API.ExceptionHandler
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
        public string? Id { get; set; }
        public string? TrackId { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string? UserFriendlyMessage { get; set; }
        public string? Ressource { get; set; }

    }
}