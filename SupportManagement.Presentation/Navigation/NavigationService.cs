using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.Views;
using System;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    [ObservableProperty]
    public partial Page? CurrentPage {  get; private set; }

    public void Navigate(AppPage page)
    {
        CurrentPage = page switch
        {
            AppPage.Customers => services.GetRequiredService<CustomersPage>(),
            AppPage.CreateCustomers => services.GetRequiredService<CreateCustomerPage>(),
            AppPage.EditCustomer => services.GetRequiredService<EditCustomerPage>(),
            AppPage.TicketOverview => services.GetRequiredService<TicketOverviewPage>(),
            AppPage.CreateTicket => services.GetRequiredService<CreateTicketPage>(),
            AppPage.Home => services.GetRequiredService<HomePage>(),
            AppPage.ManageTicket => services.GetRequiredService<ManageTicketPage>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }

    public void NavigateToTicketDetails(Guid ticketId)
    {
        var page = services.GetRequiredService<TicketDetailsPage>();

        page.ViewModel.Initialize(ticketId);

        CurrentPage = page;
    }
    public async Task NavigateToManageTicketAsync(Guid ticketId)
    {
        var page = services.GetRequiredService<ManageTicketPage>();

        await page.LoadTicketAsync(ticketId);

        CurrentPage = page;
    }
}
