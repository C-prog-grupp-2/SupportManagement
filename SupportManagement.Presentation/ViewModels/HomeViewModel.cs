using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Presentation.Navigation;

namespace SupportManagement.Presentation.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public HomeViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void GoToCustomers()
    {
        _navigationService.Navigate(AppPage.Customers);
    }

    [RelayCommand]
    private void GoToTickets()
    {
        _navigationService.Navigate(AppPage.TicketOverview);
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigationService.Navigate(AppPage.TicketOverview);
    }
}