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
    Task AssignTechnicianAsync(
       Guid ticketId,
       string technician);

    Task ChangePriorityAsync(
        Guid ticketId,
        TicketPriority priority);

    Task ChangeStatusAsync(
        Guid ticketId,
        TicketStatus status);

    Task AddCommentAsync(
        Guid ticketId,
        string commentText);

    Task UpdateDetailsAsync(
        Guid ticketId,
        string title,
        string description);
}
