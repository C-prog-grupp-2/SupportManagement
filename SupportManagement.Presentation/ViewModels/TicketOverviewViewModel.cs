
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using SupportManagement.Presentation.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class TicketOverviewViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerService _customerService;

    public TicketOverviewViewModel(
        ITicketService ticketService,
        ICustomerService customerService,
        INavigationService navigationService)
    {
        _ticketService = ticketService;
        _customerService = customerService;
        _navigationService = navigationService;
    }

    public ObservableCollection<TicketOverviewItemViewModel> OpenTickets
    { get; } = [];

    public async Task LoadTicketsAsync()
    {
        IsLoading = true;
        StatusMessage = string.Empty;

        try
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            var customers = await _customerService.GetAllAsync();

            OpenTickets.Clear();

            foreach (var ticket in tickets)
            {
                if (ticket.Status == TicketStatus.Resolved)
                {
                    continue;
                }

                var customer = customers.FirstOrDefault(
                    customer => customer.Id == ticket.CustomerId);

                OpenTickets.Add(new TicketOverviewItemViewModel(
                    ticket,
                    customer?.Name ?? "Okänd kund"));
            }

            OpenTicketCount = OpenTickets.Count;

            NewTicketCount = OpenTickets.Count(
                ticket => ticket.Status == TicketStatus.New);

            HighPriorityTicketCount = OpenTickets.Count(
                ticket => ticket.Priority == TicketPriority.High);
        }
        catch (Exception)
        {
            StatusMessage =
                "Ärendeöversikten kunde inte hämtas. Försök igen.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [ObservableProperty]
    public partial int OpenTicketCount { get; private set; }

    [ObservableProperty]
    public partial int NewTicketCount { get; set; }

    [ObservableProperty]
    public partial int HighPriorityTicketCount { get; set; }

    [ObservableProperty]
    public partial string StatusMessage { get; private set; }
        = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [RelayCommand]
    private void NavigateToTicketRegistration()
    {
        _navigationService.Navigate(AppPage.CreateTicket);
    }

    // Behåller den befintliga navigeringen till detaljsidan.
    [RelayCommand]
    private void OpenTicketDetails(Guid ticketId)
    {
        _navigationService.NavigateToTicketDetails(ticketId);
    }
}
