using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using SupportManagement.Presentation.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class ManageTicketViewModel(
    ITicketService _ticketService,
    ICustomerService _customerService,
    INavigationService _navigationService) : ObservableObject
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
        ErrorMessage = string.Empty;

        // Rensa tidigare data innan ett nytt ärende laddas.
        Ticket = null;
        Customer = null;
        SelectedTechnician = null;
        CommentText = string.Empty;

        try
        {
            var ticket = await _ticketService.GetTicketAsync(ticketId);

            if (ticket is null)
            {
                ErrorMessage = "Ärendet kunde inte hittas.";
                return;
            }

            var customers = await _customerService.GetAllAsync();

            // Sortera kommentarer från äldst till nyast.
            ticket.Comments = ticket.Comments
                .OrderBy(comment => comment.CommentCreatedAtUtc)
                .ToList();

            Ticket = ticket;

            // Hämta kunden som hör till ärendet.
            Customer = customers.FirstOrDefault(
                customer => customer.Id == ticket.CustomerId);

            // Fyll formuläret med ärendets aktuella värden.
            SelectedPriority = ticket.Priority;
            SelectedStatus = ticket.Status;
            SelectedTechnician = ticket.ResponsibleTechnician;
        }
        catch (Exception)
        {
            Ticket = null;
            Customer = null;

            ErrorMessage =
                "Ärendet kunde inte hämtas. Försök igen.";
        }
    }

    public async Task SaveAsync()
    {
        ErrorMessage = string.Empty;

        if (Ticket is null)
        {
            ErrorMessage = "Inget ärende har laddats.";
            return;
        }

        // Kontrollera status och tekniker innan något sparas.
        if ((SelectedStatus == TicketStatus.InProgress ||
             SelectedStatus == TicketStatus.Resolved) &&
            string.IsNullOrWhiteSpace(SelectedTechnician))
        {
            ErrorMessage =
                "Välj en ansvarig tekniker innan ärendet sätts till Pågående eller Löst.";
            return;
        }

        try
        {
            var ticketId = Ticket.Id;

            // Validera och spara alla ändringar tillsammans.
            await _ticketService.UpdateTicketAsync(
                ticketId,
                Ticket.Title,
                Ticket.Description,
                SelectedPriority,
                SelectedTechnician,
                SelectedStatus);

            // Hämta de uppdaterade uppgifterna.
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
            ErrorMessage =
                "Det gick inte att spara ärendet. Försök igen.";
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
            ErrorMessage =
                "Skriv en kommentar innan du lägger till den.";
            return;
        }

        try
        {
            var ticketId = Ticket.Id;

            await _ticketService.AddCommentAsync(
                ticketId,
                CommentText);

            CommentText = string.Empty;

            // Ladda om ärendet så att den nya kommentaren visas.
            await LoadTicketAsync(ticketId);
        }
        catch (ArgumentException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (Exception)
        {
            ErrorMessage =
                "Det gick inte att lägga till kommentaren. Försök igen.";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigationService.Navigate(AppPage.TicketOverview);
    }
}