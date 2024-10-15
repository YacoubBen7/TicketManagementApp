using Microsoft.EntityFrameworkCore;
using TicketService.Domain.Models;

namespace TicketService.Infrastructure.Data
{
    public interface ITicketDbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
