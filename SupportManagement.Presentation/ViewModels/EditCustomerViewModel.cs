using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using System;
using SupportManagement.Domain.Models;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class EditCustomerViewModel(ICustomerService customerService) : ObservableObject
{
    [ObservableProperty]
    public partial Guid CustomerId { get; private set; }

    [ObservableProperty]
    public partial string CustomerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] 
    public partial string StatusMessage { get; private set; } = string.Empty; //Visar validerings- eller felmeddelanden i GUI:t.
    
    
    
    public void LoadCustomer(Customer customer) // Fyller Edit-formuläret med den valda kundens uppgifter. 
    {
        CustomerId = customer.Id;
        CustomerName = customer.Name;
        Email = customer.Email;

    
    }

    //Uppdaterar kundens uppgifter och hanterar eventuella fel från CustomerService.
    public async Task SaveAsync()
    {
        StatusMessage = string.Empty;

        try
        {
            await customerService.UpdateAsync(CustomerId, CustomerName, Email); //await för att UpdateAsync ska läsa & skriva Json-filen.

        }
        catch (ArgumentException ex) //visar valideringsfel som användaren kan rätta (ex tomt namn,felaktig mejl).
        {
            StatusMessage = ex.Message; 

        }
        catch (Exception) //Hanterar andra fel, tex problem med kundfilen.
        {

            StatusMessage = "Kunden kunde inte uppdateras. Kontrollera kundfilen och försök igen";
        }


    }
}

