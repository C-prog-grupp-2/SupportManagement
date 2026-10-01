using SupportManagement.Domain.Models;

namespace SupportManagement.Application.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(); // Hämtar alla kunder och returnerar dem som en lista.

    Task SaveAllAsync(IEnumerable<Customer> customers); //Tar emot en samling med kunder som heter customers och sparar dem.
}
