using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;

namespace SupportManagement.Application.Services;

public class TicketService(ITicketRepository ticketRepository, ICustomerRepository customerRepository) : ITicketService
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

        if (!customers.Any(customer  => customer.Id == customerId ))
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
}
