using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Services;

// TicketService innehåller funktioner för att ändra tickets.
public class TicketService(ITicketRepository repository)
{
    // Tilldelar en tekniker till ett ärende.
    public async Task AssignTechnicianAsync(
        Guid ticketId,
        string technician)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await repository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Tilldelar teknikern till ärendet.
        ticket.AssignTechnician(technician);

        // Sparar det uppdaterade ärendet.
        await repository.SaveAsync(ticket);
    }

// Ändrar prioriteten på ett ärende.
public async Task ChangePriorityAsync(
    Guid ticketId,
    TicketPriority priority)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await repository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar ärendets prioritet.
        ticket.ChangePriority(priority);

        // Sparar det uppdaterade ärendet.
        await repository.SaveAsync(ticket);
    }
    // Ändrar statusen på ett ärende.
    public async Task ChangeStatusAsync(
        Guid ticketId,
        TicketStatus status)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await repository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar ärendets status.
        ticket.ChangeStatus(status);

        // Sparar det uppdaterade ärendet.
        await repository.SaveAsync(ticket);
    }
    // Lägger till en kommentar på ett ärende.
    public async Task AddCommentAsync(
        Guid ticketId,
        string commentText)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await repository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Lägger till kommentaren på ärendet.
        ticket.AddComment(commentText);

        // Sparar ärendet med den nya kommentaren.
        await repository.SaveAsync(ticket);
    }
}