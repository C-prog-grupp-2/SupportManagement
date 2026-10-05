using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Interfaces;

public interface ITicketRepository
{
    Task SaveAsync(Ticket ticket);

    // Hämtar ett ärende med hjälp av dess ID.
    Task<Ticket?> GetByIdAsync(Guid ticketId);
}
