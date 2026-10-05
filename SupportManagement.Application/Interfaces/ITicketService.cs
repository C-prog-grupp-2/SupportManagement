using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;


namespace SupportManagement.Application.Interfaces;

public interface ITicketService
{
    Task<Ticket> RegisterTicketAsync(
    string title,
    string description,
    Guid customerId,
    TicketPriority priority);
}
