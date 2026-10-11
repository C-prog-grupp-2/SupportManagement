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

    Task<Ticket?> GetTicketAsync(Guid ticketId);

    Task<List<Ticket>> GetAllTicketsAsync();

    Task AddCommentAsync(
        Guid ticketId,
        string commentText);

    Task UpdateTicketAsync(
        Guid ticketId,
        string title,
        string description,
        TicketPriority priority,
        string? technician,
        TicketStatus status);
}
