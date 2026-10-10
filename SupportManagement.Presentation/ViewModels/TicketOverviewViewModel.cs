using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using SupportManagement.Presentation.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace SupportManagement.Presentation.ViewModels;

public partial class TicketOverviewViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly INavigationService _navigationService;

    public TicketOverviewViewModel(ITicketService ticketService, INavigationService navigationService)
    {
        _ticketService = ticketService;
        _navigationService = navigationService;
    }

    public ObservableCollection<Ticket> OpenTickets { get; } = [];

    public async Task LoadTicketsAsync()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();

        OpenTickets.Clear();

        foreach (var ticket in tickets)
        {
            if (ticket.Status != TicketStatus.Resolved)
            {
                OpenTickets.Add(ticket);
            }
        }

        OpenTicketCount = OpenTickets.Count;
        
        NewTicketCount = OpenTickets.Count(ticket => ticket.Status == TicketStatus.New);

        HighPriorityTicketCount = OpenTickets.Count(ticket => ticket.Priority == TicketPriority.High);
    }

    [ObservableProperty]
    public partial int OpenTicketCount { get; private set; }

    [ObservableProperty]
    public partial int NewTicketCount { get; set; }

    [ObservableProperty]
    public partial int HighPriorityTicketCount { get; set; }

    [RelayCommand]
    private void NavigateToTicketRegistration()
    {
        _navigationService.Navigate(AppPage.CreateTicket);
    }

    [RelayCommand]
    private void NavigateToTicketManagement()
    {
        _navigationService.Navigate(AppPage.ManageTicket);
    }
}

