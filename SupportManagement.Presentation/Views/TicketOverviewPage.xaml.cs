using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System;


namespace SupportManagement.Presentation.Views;

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

