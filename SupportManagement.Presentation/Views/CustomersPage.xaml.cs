using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace SupportManagement.Presentation.Views;

public sealed partial class CustomersPage : Page
{
    // Sidan får tillgång till CustomersViewModel
    public CustomersViewModel ViewModel { get; }

    //DI skickar in den när sidan skapas.
    public CustomersPage(CustomersViewModel viewModel) 
    {
        ViewModel = viewModel; //ViewModel sparar den så att XAML kan använda den.
        InitializeComponent(); // Laddar sidans XAML.

        Loaded += CustomersPage_Loaded; //Event kommer igång när sidan laddas (metoden nedan) 
    
    }

    //Hämtar kunden när sidan öppnas.
    private async void CustomersPage_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadCustomersAsync();
    }
}
