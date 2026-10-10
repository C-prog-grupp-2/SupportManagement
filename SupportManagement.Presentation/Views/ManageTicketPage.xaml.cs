using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.Views;

public sealed partial class ManageTicketPage : Page
{
    public ManageTicketViewModel ViewModel { get; }

    public ManageTicketPage(ManageTicketViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    public async Task LoadTicketAsync(Guid ticketId)
    {
        await ViewModel.LoadTicketAsync(ticketId);
    }

    private async void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.SaveAsync();
    }

    private async void AddCommentButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.AddCommentAsync();
    }
}