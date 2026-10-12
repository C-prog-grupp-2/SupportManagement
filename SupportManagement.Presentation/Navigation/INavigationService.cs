using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    Page? CurrentPage { get; }

    void Navigate(AppPage page);

    void NavigateToTicketDetails(Guid ticketId);

    Task NavigateToManageTicketAsync(Guid ticketId);
}