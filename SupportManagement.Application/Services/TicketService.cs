using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Services;


public class TicketService(
    ITicketRepository ticketRepository,
    ICustomerRepository customerRepository) : ITicketService
{
    
    public async Task<Ticket> RegisterTicketAsync(
        string title,
        string description,
        Guid customerId,
        TicketPriority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Ange en rubrik.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Ange en beskrivning av problemet", nameof(description));

        if (!Enum.IsDefined(priority))
            throw new ArgumentException("Ange en giltig prioritet.", nameof(priority));

        var customers = await customerRepository.GetAllAsync();

        if (!customers.Any(customer => customer.Id == customerId))
            throw new ArgumentException("Välj en registrerad kund.", nameof(customerId));

        var ticket = new Ticket
        {
            Title = title.Trim(),
            Description = description.Trim(),
            CustomerId = customerId,
            Priority = priority
        };

        await ticketRepository.SaveAsync(ticket);

        return ticket;
    }

    public async Task AssignTechnicianAsync(
        Guid ticketId,
        string technician)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await ticketRepository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Tilldelar teknikern till ärendet.
        ticket.AssignTechnician(technician);

        // Sparar det uppdaterade ärendet.
        await ticketRepository.SaveAsync(ticket);
    }

    public async Task ChangePriorityAsync(
        Guid ticketId,
        TicketPriority priority)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await ticketRepository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar ärendets prioritet.
        ticket.ChangePriority(priority);

        // Sparar det uppdaterade ärendet.
        await ticketRepository.SaveAsync(ticket);
    }

    public async Task ChangeStatusAsync(
        Guid ticketId,
        TicketStatus status)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await ticketRepository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar ärendets status.
        ticket.ChangeStatus(status);

        // Sparar det uppdaterade ärendet.
        await ticketRepository.SaveAsync(ticket);
    }

    public async Task AddCommentAsync(
        Guid ticketId,
        string commentText)
    {
        // Hämtar ärendet med hjälp av dess ID.
        var ticket = await ticketRepository.GetByIdAsync(ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Lägger till kommentaren på ärendet.
        ticket.AddComment(commentText);

        // Sparar ärendet med den nya kommentaren.
        await ticketRepository.SaveAsync(ticket);
    }
}