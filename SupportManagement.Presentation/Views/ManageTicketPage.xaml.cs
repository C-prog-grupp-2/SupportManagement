using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportManagement.Presentation.ViewModels;
using System;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.Views;

public sealed partial class ManageTicketPage : Page
{
    private readonly ManageTicketViewModel _viewModel;

    public ManageTicketPage(ManageTicketViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;
    }

    public async Task LoadTicketAsync(Guid ticketId)
    {
        await _viewModel.LoadTicketAsync(ticketId);
    }

    private async void SaveButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.SaveAsync();
    }

    private async void AddCommentButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.AddCommentAsync();
    }
}