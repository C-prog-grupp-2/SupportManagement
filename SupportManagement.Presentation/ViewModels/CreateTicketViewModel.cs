using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Presentation.Navigation;
using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;

namespace SupportManagement.Presentation.ViewModels;

public partial class CreateTicketViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly ICustomerService _customerService;
    private readonly INavigationService _navigationService;

    public IReadOnlyList<TicketPriority> Priorities { get; } = Enum.GetValues<TicketPriority>();

    public CreateTicketViewModel(
        ITicketService ticketService,
        ICustomerService customerService,
        INavigationService navigationService)
    {
        _ticketService = ticketService;
        _customerService = customerService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }

    [ObservableProperty]
    public partial TicketPriority SelectedPriority { get; set; } = TicketPriority.Normal;

    [ObservableProperty]
    public partial string StatusMessage { get; set; } = string.Empty;

    public ObservableCollection<Customer> Customers { get; } = [];

    public async Task LoadCustomersAsync()
    {
        StatusMessage = string.Empty;

        try
        {
            var customers = await _customerService.GetAllAsync();

            Customers.Clear();

            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            if (Customers.Count == 0)
            {
                StatusMessage = "Registrera en kund innan du skapar ett ärende.";
            }
        }
        catch (Exception)
        {
            StatusMessage = "Kunderna kunde inte hämtas. Försök igen.";
        }
    }

    [RelayCommand]
    private async Task RegisterTicketAsync()
    {
        StatusMessage = string.Empty;

        if (SelectedCustomer is null)
        {
            StatusMessage = "Välj en giltig kund.";
            return;
        }

        try
        {
            await _ticketService.RegisterTicketAsync(
                Title,
                Description,
                SelectedCustomer.Id,
                SelectedPriority);
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
            return;
        }
        catch (Exception)
        {
            StatusMessage = "Ärendet kunde inte sparas. Försök igen.";
            return;
        }

        _navigationService.Navigate(AppPage.TicketOverview);
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigationService.Navigate(AppPage.TicketOverview);
    }
}

