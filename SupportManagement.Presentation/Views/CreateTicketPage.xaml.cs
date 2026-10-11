using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;

namespace SupportManagement.Presentation.Views;

public sealed partial class CreateTicketPage : Page
{
    public CreateTicketViewModel ViewModel { get; }

    public CreateTicketPage(CreateTicketViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        Loaded += CreateTicketPage_Loaded;
    }

    private async void CreateTicketPage_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadCustomersAsync();
    }
}