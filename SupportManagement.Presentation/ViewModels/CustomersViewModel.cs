using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Media.Capture.Core;
namespace SupportManagement.Presentation.ViewModels;

public partial class CustomersViewModel (ICustomerService customerService): ObservableObject //lägg in INavigationService när den är klar?
{
    public ObservableCollection<Customer> Customers { get; } = [];

    public async Task LoadCustomersAsync()
    {
        var customers = await customerService.GetAllAsync();
    }

}
