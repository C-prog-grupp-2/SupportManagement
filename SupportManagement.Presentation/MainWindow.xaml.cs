using Microsoft.UI.Xaml;

namespace SupportManagement.Presentation;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        // Laddar gränssnittet från MainWindow.xaml.
        InitializeComponent();

        // Visar TicketDetailView i appens Frame.
        ContentFrame.Navigate(typeof(Views.TicketDetailView));
    }
}