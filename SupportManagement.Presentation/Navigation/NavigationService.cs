using CommunityToolkit.Mvvm.ComponentModel;
//using SupportManagement.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System;

namespace SupportManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    [ObservableProperty]
    public partial Page? CurrentPage {  get; private set; }

    public void Navigate(AppPage page)
    {
        CurrentPage = page switch
        {
            //AppPage.Customers => services.GetRequiredService<CustomersPage>(),
            //AppPage.CreateCustomers => services.GetRequiredService<CreateCustomerPage>(),
            //AppPage.EditCustomer => services.GetRequiredService<EditCustomerPage>(),
            //AppPage.TicketOverview => services.GetRequiredService<TicketOverviewPage>(),
            //AppPage.CreateTicket => services.GetRequiredService<CreateTicketPage>(),
            AppPage.ManageTicket => services.GetRequiredService<ManageTicketPage>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }
}
