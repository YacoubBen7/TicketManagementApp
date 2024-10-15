using Microsoft.Extensions.Logging;
using TicketService.Core.ServicesContracts;
using TicketService.Domain.Models;
using TicketService.Infrastructure.Repositories;

namespace TicketService.Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ILogger<TicketService> _logger;

        public TicketService(ITicketRepository ticketRepository, ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        public async Task<Ticket> AddAsync(Ticket entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _logger.LogInformation("Adding ticket with id {id}", entity.Id);
            return await _ticketRepository.AddAsync(entity);
        }

        public Task<int> CountAsync(string filterQuery)
        {
            _logger.LogInformation("Counting tickets");
            return _ticketRepository.CountAsync(filterQuery);
        }

        public Task<int> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting ticket with id {id}", id);
            return _ticketRepository.DeleteAsync(id);
        }

        public async Task<List<Ticket>> GetAllAsync(
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterQuery
        )
        {
            _logger.LogInformation("Getting all tickets");
            return await _ticketRepository.GetAllAsync(
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterQuery
            );
        }

        public Task<Ticket> GetAsync(int id)
        {
            _logger.LogInformation("Getting ticket");
            return _ticketRepository.GetAsync(id)!;
        }

        public Task<int> UpdateAsync(Ticket entity)
        {
            _logger.LogInformation("Updating ticket with id {id}", entity.Id);
            return _ticketRepository.UpdateAsync(entity);
        }
    }
}
