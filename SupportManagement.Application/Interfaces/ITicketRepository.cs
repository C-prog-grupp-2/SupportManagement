using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Interfaces;

public interface ITicketRepository
{
    Task SaveAsync(Ticket ticket);
}
