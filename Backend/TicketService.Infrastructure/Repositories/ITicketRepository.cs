using TicketService.Domain.Models;

namespace TicketService.Infrastructure.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> GetAsync(int id);
        Task<List<Ticket>> GetAllAsync(
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterQuery
        );
        Task<Ticket> AddAsync(Ticket entity);
        Task<int> UpdateAsync(Ticket entity);
        Task<int> DeleteAsync(int id);
        Task<int> CountAsync(string filterQuery);
    }
}
