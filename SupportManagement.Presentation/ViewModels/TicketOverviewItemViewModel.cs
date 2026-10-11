using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using System;

namespace SupportManagement.Presentation.ViewModels;

public class TicketOverviewItemViewModel
{
    private readonly Ticket _ticket;

    public TicketOverviewItemViewModel(
        Ticket ticket,
        string customerName)
    {
        _ticket = ticket;
        CustomerName = customerName;
    }

    public Guid Id => _ticket.Id;
    public Guid CustomerId => _ticket.CustomerId;
    public string Title => _ticket.Title;
    public TicketStatus Status => _ticket.Status;
    public TicketPriority Priority => _ticket.Priority;

    public string CustomerName { get; }
}
