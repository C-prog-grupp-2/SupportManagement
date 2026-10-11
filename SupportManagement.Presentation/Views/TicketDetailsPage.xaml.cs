using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;

namespace SupportManagement.Presentation.Views;

public sealed partial class TicketDetailsPage : Page
{
    public TicketDetailsViewModel ViewModel { get; }

    public TicketDetailsPage(TicketDetailsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        Loaded += TicketDetailsPage_Loaded;
    }

    private async void TicketDetailsPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadTicketAsync();
    }
}
