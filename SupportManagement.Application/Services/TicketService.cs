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

        var tickets = await ticketRepository.GetAllAsync();
        tickets.Add(ticket);
        await ticketRepository.SaveAllAsync(tickets);

        return ticket;
    }
    public async Task<Ticket?> GetTicketAsync(Guid ticketId)
    {
        var tickets = await ticketRepository.GetAllAsync();

        return tickets.FirstOrDefault(ticket => ticket.Id == ticketId);
    }

    // Tilldelar en tekniker till ett ärende.
    public async Task AssignTechnicianAsync(
        Guid ticketId,
        string technician)
    {
        // Hämtar alla ärenden.
        var tickets = await ticketRepository.GetAllAsync();

        // Letar efter det valda ärendet.
        var ticket = tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Tilldelar teknikern.
        ticket.AssignTechnician(technician);

        // Sparar hela listan.
        await ticketRepository.SaveAllAsync(tickets);
    }

    // Ändrar prioriteten på ett ärende.
    public async Task ChangePriorityAsync(
        Guid ticketId,
        TicketPriority priority)
    {
        // Hämtar alla ärenden.
        var tickets = await ticketRepository.GetAllAsync();

        // Letar efter det valda ärendet.
        var ticket = tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar prioriteten.
        ticket.ChangePriority(priority);

        // Sparar hela listan.
        await ticketRepository.SaveAllAsync(tickets);
    }

    // Ändrar statusen på ett ärende.
    public async Task ChangeStatusAsync(
        Guid ticketId,
        TicketStatus status)
    {
        // Hämtar alla ärenden.
        var tickets = await ticketRepository.GetAllAsync();

        // Letar efter det valda ärendet.
        var ticket = tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Ändrar statusen.
        ticket.ChangeStatus(status);

        // Sparar hela listan.
        await ticketRepository.SaveAllAsync(tickets);
    }

    // Lägger till en kommentar på ett ärende.
    public async Task AddCommentAsync(
        Guid ticketId,
        string commentText)
    {
        // Hämtar alla ärenden.
        var tickets = await ticketRepository.GetAllAsync();

        // Letar efter det valda ärendet.
        var ticket = tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Lägger till kommentaren.
        ticket.AddComment(commentText);

        // Sparar hela listan.
        await ticketRepository.SaveAllAsync(tickets);
    }

    // Uppdaterar rubrik och beskrivning på ett ärende.
    public async Task UpdateDetailsAsync(
        Guid ticketId,
        string title,
        string description)
    {
        // Hämtar alla ärenden.
        var tickets = await ticketRepository.GetAllAsync();

        // Letar efter det valda ärendet.
        var ticket = tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

        // Om ärendet inte hittas kastas ett fel.
        if (ticket is null)
            throw new InvalidOperationException("Ärendet hittades inte.");

        // Uppdaterar rubrik och beskrivning.
        ticket.UpdateDetails(title, description);

        // Sparar hela listan.
        await ticketRepository.SaveAllAsync(tickets);
    }
}