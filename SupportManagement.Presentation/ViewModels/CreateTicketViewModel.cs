using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Presentation.Navigation;
using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class CreateTicketViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly ICustomerService _customerService;
    private readonly INavigationService _navigationService;

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
        var customers = await _customerService.GetAllAsync();
        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(customer);
        }
    }
}

