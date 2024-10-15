
namespace TicketService.Domain.Exception
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string entity)
            : base(
                $"The {entity} is invalid.",
                "BAD_REQUEST",
                "The request is invalid."
            )
        { }
    }
}