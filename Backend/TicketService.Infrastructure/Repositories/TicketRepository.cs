using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketService.Domain.Exception;
using TicketService.Domain.Models;
using TicketService.Infrastructure.Data;

namespace TicketService.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ITicketDbContext _dbContext;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(ITicketDbContext dbContext, ILogger<TicketRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Ticket> AddAsync(Ticket entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _logger.LogInformation("Adding ticket with id {id} in The Db", entity.Id);
            var ticketInDb = _dbContext.Tickets.Add(entity);
            await _dbContext.SaveChangesAsync();
            return ticketInDb.Entity;
        }

        public async Task<int> CountAsync(string filterQuery)
        {
            _logger.LogInformation("Counting tickets from The Db");
            return await _dbContext
                .Tickets.Where(t =>
                    String.IsNullOrEmpty(filterQuery) || t.Description!.Contains(filterQuery)
                )
                .CountAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting ticket with id {id} from The Db", id);
            var ticket = await _dbContext.Tickets.FindAsync(id);
            if (ticket == null)
                return -1;
            _dbContext.Tickets.Remove(ticket);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetAllAsync(
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterQuery
        )
        {

            _logger.LogInformation("Getting all tickets from The Db");
            return await _dbContext
                .Tickets.Where(t =>
                    String.IsNullOrEmpty(filterQuery)
                    || t.Description!.Contains(filterQuery)
                    || t.Id.ToString().Contains(filterQuery)
                )
                .OrderBy($"{sortColumn} {sortOrder}")
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Ticket> GetAsync(int id)
        {
            _logger.LogInformation("Getting ticket from The Db");
            var ticket = await _dbContext.Tickets.FindAsync(id);
            NotFoundException.ThrowIfNull(ticket, id, "Ticket");
            return ticket!;
        }

        public async Task<int> UpdateAsync(Ticket entity)
        {
            _logger.LogInformation("Updating ticket with id {id} in The Db", entity.Id);
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            var existingTicket = await _dbContext.Tickets.FindAsync(entity.Id);
            NotFoundException.ThrowIfNull(existingTicket, entity.Id, "Ticket");

            existingTicket!.Description = entity.Description;
            existingTicket!.Status = entity.Status;

            return await _dbContext.SaveChangesAsync();
        }
    }
}
