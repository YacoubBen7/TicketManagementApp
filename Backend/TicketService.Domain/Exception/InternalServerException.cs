namespace TicketService.Domain.Exception
{
    public class InternalServerException : BaseException
    {
        public InternalServerException(string id, string entity)
            : base(
                $"An internal server error occurred while processing the {entity} with ID {id}.",
                "INTERNAL_SERVER_ERROR",
                id,
                "An internal server error occurred."
            )
        { }

    }
}