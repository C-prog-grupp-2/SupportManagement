using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Interfaces;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync();
    Task SaveAllAsync(IEnumerable<Ticket> tickets);
}