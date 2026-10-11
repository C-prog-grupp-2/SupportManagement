using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;

namespace SupportManagement.Presentation.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel { get; }

    public HomePage(HomeViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}
