using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using SupportManagement.Presentation.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class TicketDetailsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerService _customerService;
    private Guid _ticketId;

    public string CommentsHeading =>
        Ticket is null
            ? "Kommentarer"
            : Ticket.Comments.Count == 0
                ? "Inga kommentarer har lagts till"
                : "Kommentarer";

    public TicketDetailsViewModel(
        ITicketService ticketService,
        ICustomerService customerService,
        INavigationService navigationService)
    {
        _ticketService = ticketService;
        _customerService = customerService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CommentsHeading))]
    public partial Ticket? Ticket { get; private set; }

    [ObservableProperty]
    public partial string StatusMessage { get; private set; }
        = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial string CustomerName { get; private set; } = string.Empty;

    public void Initialize(Guid ticketId)
    {
        _ticketId = ticketId;
    }

    public async Task LoadTicketAsync()
    {
        IsLoading = true;
        StatusMessage = string.Empty;
        Ticket = null;
        CustomerName = string.Empty;

        try
        {
            Ticket = await _ticketService.GetTicketAsync(_ticketId);

            if (Ticket is null)
            {
                StatusMessage = "Ärendet hittades inte.";
                return;
            }

            var customers = await _customerService.GetAllAsync();

            CustomerName = customers.FirstOrDefault(
                customer => customer.Id == Ticket.CustomerId)?.Name
                ?? "Okänd kund";
        }
        catch (Exception)
        {
            StatusMessage = "Ärendet kunde inte hämtas. Försök igen.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigationService.Navigate(AppPage.TicketOverview);
    }

    // Öppnar ManageTicket för det ärende som visas här.
    [RelayCommand]
    private async Task OpenManageTicketAsync()
    {
        var ticket = Ticket;

        if (ticket is null)
        {
            StatusMessage = "Ärendet har inte laddats.";
            return;
        }

        await _navigationService.NavigateToManageTicketAsync(ticket.Id);
    }

    public void OpenTicketDetails(Guid ticketId)
    {
        _navigationService.NavigateToTicketDetails(ticketId);
    }
}