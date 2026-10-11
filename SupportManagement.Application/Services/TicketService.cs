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

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        return await ticketRepository.GetAllAsync();
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

    public async Task UpdateTicketAsync(
        Guid ticketId,
        string title,
        string description,
        TicketPriority priority,
        string? technician,
        TicketStatus status)
    {
        var tickets = await ticketRepository.GetAllAsync();

        var ticket = tickets.FirstOrDefault(
            ticket => ticket.Id == ticketId);

        if (ticket is null)
        {
            throw new InvalidOperationException("Ärendet hittades inte.");
        }

        ticket.UpdateDetails(title, description);
        ticket.ChangePriority(priority);

        if (!string.IsNullOrWhiteSpace(technician))
        {
            ticket.AssignTechnician(technician);
        }

        ticket.ChangeStatus(status);

        await ticketRepository.SaveAllAsync(tickets);
    }
}