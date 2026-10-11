using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System;
using System.Security.Cryptography.X509Certificates;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SupportManagement.Presentation.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class TicketOverviewPage : Page
{
    public TicketOverviewViewModel ViewModel { get; }

    public TicketOverviewPage(TicketOverviewViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();

        Loaded += TicketOverviewPage_Loaded;
    }

    private async void TicketOverviewPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadTicketsAsync();
    }

    private void OpenTickets_ItemClick(
    object sender,
    ItemClickEventArgs e)
    {
        if (e.ClickedItem is TicketOverviewItemViewModel ticket)
        {
            ViewModel.OpenTicketDetailsCommand.Execute(ticket.Id);
        }
    }
}

