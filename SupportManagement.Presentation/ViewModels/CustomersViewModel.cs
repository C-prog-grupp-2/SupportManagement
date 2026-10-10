using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using SupportManagement.Presentation.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace SupportManagement.Presentation.ViewModels;


public partial class CustomersViewModel(ICustomerService customerService, INavigationService navigationService, EditCustomerViewModel editCustomerViewModel) : ObservableObject 
{
    //GUI:t kan uppdatera när kunder läggs till/tas bort.
    public ObservableCollection<Customer> Customers { get; } = []; // Samlingen kan inte ersättas men innehållet kan ändras. Startar tom.


    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }  //Håller reda på vilken kund som är markerad i listan.


    [ObservableProperty]
    public partial string StatusMessage { get; private set; } = string.Empty;   //Visar felmeddelanden i Gui:t.


    public async Task LoadCustomersAsync() //Laddar kunder asynkront
    {
        StatusMessage = string.Empty;

        try
        {

            var customers = await customerService.GetAllAsync(); //Anropar metoden från Servicens List<Customer>, vi sparar den till lokala variabeln customers.

            Customers.Clear(); //Tömmer GUI:s nuvarande kundlista så att man inte lägger in samma kunder flera ggr om man laddar om.

            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

        catch (Exception)
        {
            StatusMessage = "Kunderna kunde inte läsas in. Kontrollera kundfilen.";
        }
    }

    [RelayCommand]
    public async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer == null)
        {
            StatusMessage = "Markera en kund som du vill ta bort";
            return;
        
        }

        StatusMessage = string.Empty;

        try
        {

            await customerService.DeleteAsync(SelectedCustomer.Id); //Ta den markerade kunden, be service radera den, ladda om listan.
            await LoadCustomersAsync();
        }
        catch (InvalidOperationException ex)
        {
            StatusMessage = ex.Message;
        }
        catch (Exception)
        {
            StatusMessage = "Kunden kunde inte tas bort. Försök igen.";
        }
    }


    [RelayCommand]
    private void GoToCreateCustomer()
    {
        navigationService.Navigate(AppPage.CreateCustomers);
    }


    [RelayCommand]
    private void GoToEditCustomer()    
    {
        if (SelectedCustomer == null)
        {
            StatusMessage = "Markera en kund som du vill redigera.";
            return;
        }

        StatusMessage = string.Empty;

        editCustomerViewModel.LoadCustomer(SelectedCustomer);
        navigationService.Navigate(AppPage.EditCustomer);
    }
}
