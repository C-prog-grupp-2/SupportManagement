using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
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

    [ObservableProperty]
    private TicketPriority selectedPriority;

    [ObservableProperty]
    private TicketStatus selectedStatus;

    [ObservableProperty]
    private string? selectedTechnician;

    [ObservableProperty]
    private string commentText = string.Empty;

    public TicketPriority[] TicketPriorities { get; } =
        Enum.GetValues<TicketPriority>();

    public TicketStatus[] TicketStatuses { get; } =
        Enum.GetValues<TicketStatus>();

    public string[] Technicians { get; } =
    [
        "Anna",
        "Erik",
        "Johan"
    ];

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

        SelectedPriority = Ticket.Priority;
        SelectedStatus = Ticket.Status;
        SelectedTechnician = Ticket.ResponsibleTechnician;

        var customers = await _customerService.GetAllAsync();

        Customer = customers.FirstOrDefault(
            customer => customer.Id == Ticket.CustomerId);
    }

    public async Task SaveAsync()
    {
        if (Ticket is null)
            return;

        await _ticketService.UpdateDetailsAsync(
            Ticket.Id,
            Ticket.Title,
            Ticket.Description);

        await _ticketService.ChangePriorityAsync(
            Ticket.Id,
            SelectedPriority);

        if (!string.IsNullOrWhiteSpace(SelectedTechnician))
        {
            await _ticketService.AssignTechnicianAsync(
                Ticket.Id,
                SelectedTechnician);
        }

        await _ticketService.ChangeStatusAsync(
            Ticket.Id,
            SelectedStatus);

        await LoadTicketAsync(Ticket.Id);
    }

    public async Task AddCommentAsync()
    {
        if (Ticket is null)
            return;

        if (string.IsNullOrWhiteSpace(CommentText))
            return;

        await _ticketService.AddCommentAsync(
            Ticket.Id,
            CommentText);

        CommentText = string.Empty;

        await LoadTicketAsync(Ticket.Id);
    }
}