using TicketService.Domain.Models;

namespace TicketService.Core.ServicesContracts
{
    public interface ITicketService
    {
        Task<Ticket> GetAsync(int id);
        Task<List<Ticket>> GetAllAsync(
            int pageIndex,
            int pageSize,
            string sortColumn,
            string sortOrder,
            string filterQuery
        );
        Task<int> CountAsync(string filterQuery);
        Task<Ticket> AddAsync(Ticket entity);
        Task<int> UpdateAsync(Ticket entity);
        Task<int> DeleteAsync(int id);
    }
}
