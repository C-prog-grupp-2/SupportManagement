using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using System.Net.Mail;


namespace SupportManagement.Application.Services;

public class CustomerService (ICustomerRepository customerRepository) : ICustomerService
{
    public Task<List<Customer>> GetAllAsync() 
    {
        return customerRepository.GetAllAsync();//ber repositoryn hämta alla kunder & returnera samma Task vidare.
    }

    public async Task CreateAsync(string name, string email)
    {
        //Kontrollera input namn om den är tom eller har blanksteg.Generera felmeddelande.
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Ange kundens namn eller företagsnamn.");
        

        //Kontrollera input email om den är tom eller har blanksteg.
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Ange en e-postadress.");

        name = name.Trim(); 
        email = email.Trim();

        //Kontrollera att e-postadressen har ett giltigt format.
        if (!MailAddress.TryCreate(email, out var address) || address.Address != email)
            throw new ArgumentException("Ange en giltig e-postadress.");


        var customers = await customerRepository.GetAllAsync(); //Hämta de kunder som redan finns & lägg listan i variablen customers.

        customers.Add(new Customer //lägger kunden i listan (namn & mejl)
        {
            Name = name,
            Email = email
        }); 

        await customerRepository.SaveAllAsync(customers); //spara listan - awaita tills sparningen är klar innan CreateAsync är färdig
        
        }

   
    public async Task UpdateAsync(Guid customerId, string name, string email)
    {
        //Kontrollera input namn om den är tom eller har blanksteg. Generera felmeddelande.
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ange kundens namn eller företagsnamn.");

        //Kontrollera input email om den är tom eller har blanksteg.Generera felmeddelande.
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Ange en e-postadress");

        var customers = await customerRepository.GetAllAsync();

        var customer = customers.FirstOrDefault(customer =>customer.Id == customerId);//Hitta rätt kund via Id

        if (customer == null) // Om ingen kund med detta Id hittades, avbryt.
            return;

        customer.Name = name; //ge den nytt namn & nedan email.
        customer.Email = email;

        await customerRepository.SaveAllAsync(customers);
    }



    public async Task DeleteAsync(Guid customerId)
    {
        var customers = await customerRepository.GetAllAsync();
       
        var removeCount = customers.RemoveAll(customer => customer.Id == customerId); //Tar bort kunden vars id matchar customerId & sparar antalet borttagna kunder.

        if (removeCount > 0)
            await customerRepository.SaveAllAsync(customers);
    }
}
