using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Media.Capture.Core;
namespace SupportManagement.Presentation.ViewModels;


public partial class CustomersViewModel (ICustomerService customerService): ObservableObject //lägg in INavigationService när den är klar?
{
    //ObservableCollection används så att GUI:t kan uppdatera när kunder läggs till/tas bort.
    public ObservableCollection<Customer> Customers { get; } = []; // Samlingen kan inte ersättas men innehållet kan ändras. Startar tom.


    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }

    public async Task LoadCustomersAsync() //Laddar kunder asynkront
    {
        var customers = await customerService.GetAllAsync(); //Anropar metoden från Servicens List<Customer>, vi sparar den till lokala variabeln customers.

        Customers.Clear(); //Tömmer GUI:s nuvarande kundlista så att man inte lägger in samma kunder flera ggr om man laddar om.

        foreach(var customer in customers) //loop som går igenom alla kunder som har hämtats.
        {
            Customers.Add(customer);
        }
    }


    public async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer == null)
            return;

        await customerService.DeleteAsync(SelectedCustomer.Id); //Ta den markerade kunden, be service radera den, ladda om listan.

        await LoadCustomersAsync();
    }



}
