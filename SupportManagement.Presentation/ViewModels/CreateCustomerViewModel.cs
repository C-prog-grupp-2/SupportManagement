using CommunityToolkit.Mvvm.ComponentModel;
using SupportManagement.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SupportManagement.Presentation.ViewModels;

public partial class CreateCustomerViewModel (ICustomerService customerService) : ObservableObject
{
    //CommunityToolkit generear kod som gör att Gui:t kan reagera när en property ändras.
    [ObservableProperty]
    public partial string CustomerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] 
    public partial string StatusMessage { get; private set; } = string.Empty; //Visar validerings eller felmeddelanden i GUI:t.

    
    
    public async Task SaveAsync() //Skapar en ny kund & hanterar evetuella fel från CustomerService.
    {
        StatusMessage = string.Empty;

        try
        {
            await customerService.CreateAsync(CustomerName, Email);
        }

        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message; //Visar valideringsmeddelandet från CustomerService.
        }

        catch (Exception)
        {
            //Hanterar andra fel, ex om kundfilen inte kan spara.
            StatusMessage = "Kunden kunde inte sparas. Kontrollera kundfilen och försök igen.";
        }
    }
}
