using SupportManagement.Presentation.Navigation;
using SupportManagement.Presentation.ViewModels;
using SupportManagement.Presentation.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Windowing;
using System;
using System.ComponentModel;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SupportManagement.Presentation;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    public MainWindow(MainViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();


    }
    private void NavView_SelectionChanged(
    NavigationView sender,
    NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item &&
            item.Tag is string pageName &&
            Enum.TryParse<AppPage>(pageName, out var page))
        {
            ViewModel.Navigation.Navigate(page);
        }
    }
}

