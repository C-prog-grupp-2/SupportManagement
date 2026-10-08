using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class ManageTicket : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    private Ticket? ticket;

    [ObservableProperty]
    private Customer? customer;

    public ManageTicket(
        ITicketService ticketService,
        ICustomerService customerService)
    {
        _ticketService = ticketService;
        _customerService = customerService;
    }

    public async Task LoadTicketAsync(Guid ticketId)
    {
        Ticket = await _ticketService.GetTicketAsync(ticketId);

        if (Ticket is null)
            return;

        var customers = await _customerService.GetAllAsync();

        Customer = customers.FirstOrDefault(
            customer => customer.Id == Ticket.CustomerId);
    }
}