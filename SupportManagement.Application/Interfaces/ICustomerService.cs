using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>>GetAllAsync(); //Hämtar alla kunder från repositoryt & returnerar en lista med Customers.
    Task CreateAsync(string name, string email);//Tar emot parametrarna från användaren.Customer skickar det vidare för att sparas.
    Task UpdateAsync(Guid customerId, string name, string email);//Tar emot kundens Id för att hitta rätt Customer, samt nytt namn & epost som kunden ska uppdateras med.
    Task DeleteAsync(Guid customerId); //Tar emot kundens Id för att hitta & ta bort rätt Customer.
}
