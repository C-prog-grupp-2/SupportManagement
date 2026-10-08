using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System;
using System.Security.Cryptography.X509Certificates;

namespace SupportManagement.Presentation.Views;

public sealed partial class CustomersPage : Page
{
 
    public CustomersViewModel ViewModel { get; }   // Sidan får tillgång till CustomersViewModel

    
    public CustomersPage(CustomersViewModel viewModel) //DI skickar in den när sidan skapas.
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


    private async void DeleteCustomer_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedCustomer == null)
            return;

        var dialog = new ContentDialog
        {
            Title = "Ta bort kund?",
            Content = "Är du säker på att du vill ta bort kunden?",
            PrimaryButtonText = "Ta bort",
            CloseButtonText = "Avbryt",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = this.XamlRoot
        };

        var result = await dialog.ShowAsync();

        if(result== ContentDialogResult.Primary)
        {
            await ViewModel.DeleteCustomerAsync();

        }

    }
}
