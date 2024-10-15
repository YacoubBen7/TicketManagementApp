using TicketService.Domain.Models;

namespace TicketService.Domain.Exception
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string id, string entity)
            : base(
                $"The {entity} with the id {id} was not found.",
                "NOT_FOUND",
                id,
                "The requested resource could not be found."
            ) { }

        public static async void ThrowIfFalse(Task<bool> task, string v, Guid authorId)
        {
            var result = await task;
            if (result == false)
                throw new NotFoundException(authorId.ToString(), v);
        }

        public static void ThrowIfNull(Ticket? ticket, int id, string v)
        {
            if (ticket == null)
                throw new NotFoundException(id + "", "Ticket");
        }
    }
}
