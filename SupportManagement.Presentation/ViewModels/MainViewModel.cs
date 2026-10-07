using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Presentation.Navigation;

namespace SupportManagement.Presentation.ViewModels;

public partial class MainViewModel(INavigationService navigationService) : ObservableObject
{
    public INavigationService Navigation { get; } = navigationService;
}
