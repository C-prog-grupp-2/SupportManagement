using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;


namespace SupportManagement.Presentation.Views
{
    public sealed partial class CreateCustomerPage : Page
    {
        public CreateCustomerViewModel ViewModel { get; }
        public CreateCustomerPage(CreateCustomerViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
