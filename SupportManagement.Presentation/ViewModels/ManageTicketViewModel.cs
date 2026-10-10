using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class ManageTicketViewModel(
    ITicketService _ticketService,
    ICustomerService _customerService) : ObservableObject
{
    [ObservableProperty]
    public partial Ticket? Ticket { get; set; }

    [ObservableProperty]
    public partial Customer? Customer { get; set; }

    [ObservableProperty]
    public partial TicketPriority SelectedPriority { get; set; }

    [ObservableProperty]
    public partial TicketStatus SelectedStatus { get; set; }

    [ObservableProperty]
    public partial string? SelectedTechnician { get; set; }

    [ObservableProperty]
    public partial string CommentText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

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

    public async Task LoadTicketAsync(Guid ticketId)
    {
        Ticket = await _ticketService.GetTicketAsync(ticketId);

        if (Ticket is null)
            return;

        // Sorterar kommentarerna från äldst till nyast.
        Ticket.Comments = Ticket.Comments
            .OrderBy(comment => comment.CommentCreatedAtUtc)
            .ToList();

        SelectedPriority = Ticket.Priority;
        SelectedStatus = Ticket.Status;
        SelectedTechnician = Ticket.ResponsibleTechnician;

        var customers = await _customerService.GetAllAsync();

        Customer = customers.FirstOrDefault(
            customer => customer.Id == Ticket.CustomerId);
    }

    public async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (Ticket is null)
        {
            ErrorMessage = "Inget ärende har laddats.";
            return;
        }

        try
        {
            var ticketId = Ticket.Id;
            var technician = SelectedTechnician;


            await _ticketService.UpdateDetailsAsync(
                ticketId,
                Ticket.Title,
                Ticket.Description);

            await _ticketService.ChangePriorityAsync(
                ticketId,
                SelectedPriority);


            if (!string.IsNullOrWhiteSpace(technician))
            {
                await _ticketService.AssignTechnicianAsync(
                    ticketId,
                    technician);
            }

            await _ticketService.ChangeStatusAsync(
                ticketId,
                SelectedStatus);

            await LoadTicketAsync(ticketId);
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Det gick inte att spara ärendet. Försök igen.";
        }
    }

    public async Task AddCommentAsync()
    {
        ErrorMessage = string.Empty;

        if (Ticket is null)
        {
            ErrorMessage = "Inget ärende har laddats.";
            return;
        }

        if (string.IsNullOrWhiteSpace(CommentText))
        {
            ErrorMessage = "Skriv en kommentar innan du lägger till den.";
            return;
        }

        try
        {
            await _ticketService.AddCommentAsync(
                Ticket.Id,
                CommentText);

            CommentText = string.Empty;

            await LoadTicketAsync(Ticket.Id);
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage = "Det gick inte att lägga till kommentaren. Försök igen.";
        }
    }


}