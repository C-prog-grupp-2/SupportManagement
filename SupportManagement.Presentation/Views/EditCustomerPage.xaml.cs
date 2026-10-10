using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
namespace SupportManagement.Presentation.Views;

public sealed partial class EditCustomerPage : Page
{
    public EditCustomerViewModel ViewModel { get; } //ViewModel sparas som en egenskap som vi kan använda i XAML.
 
    public EditCustomerPage (EditCustomerViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
