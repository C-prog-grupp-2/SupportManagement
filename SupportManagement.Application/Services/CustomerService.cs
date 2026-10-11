using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using System.Net.Mail;


namespace SupportManagement.Application.Services;

public class CustomerService (ICustomerRepository customerRepository, ITicketRepository ticketRepository) : ICustomerService
{
    public Task<List<Customer>> GetAllAsync() 
    {
        return customerRepository.GetAllAsync();//ber repositoryn hämta alla kunder & returnera samma Task vidare.
    }

    public async Task CreateAsync(string name, string email)
    {
        var customers = await customerRepository.GetAllAsync(); //Hämta de kunder som redan finns & lägg listan i variablen customers.

       // Skapar en ny kund och validerar namn och e-post via Customer.
        customers.Add(new Customer(name, email));

        await customerRepository.SaveAllAsync(customers); //spara listan - awaita tills sparningen är klar innan CreateAsync är färdig    
    }

   
    public async Task UpdateAsync(Guid customerId, string name, string email)
    {
        var customers = await customerRepository.GetAllAsync();

        var customer = customers.FirstOrDefault(customer =>customer.Id == customerId);//Hitta rätt kund via Id

        if (customer == null) // Om ingen kund med detta Id hittades, avbryt.
        {
            throw new ArgumentException("Kunden kunde inte hittas.");   
        }

        customer.UpdateDetails(name, email);

        await customerRepository.SaveAllAsync(customers);
    }



    public async Task DeleteAsync(Guid customerId)
    {
        var tickets = await ticketRepository.GetAllAsync();

        bool hasTickets = tickets.Any(ticket => ticket.CustomerId == customerId);

        if (hasTickets)
        {
            throw new InvalidOperationException("Kunden kan inte tas bort eftersom det finns kopplade ärenden.");
        }
        
        var customers = await customerRepository.GetAllAsync();
       
        var removeCount = customers.RemoveAll(customer => customer.Id == customerId); //Tar bort kunden vars id matchar customerId & sparar antalet borttagna kunder.

        if (removeCount > 0)
            await customerRepository.SaveAllAsync(customers);
    }
}
