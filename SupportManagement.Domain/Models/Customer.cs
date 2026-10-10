
using System.Text.Json.Serialization;

namespace SupportManagement.Domain.Models;


public class Customer
{
    [JsonInclude]
    public Guid Id { get; private set; } = Guid.NewGuid();//bara Customer-klassen får ändra Id:t o skapa den.
    
    [JsonInclude]
    public string Name { get; private set; } =string.Empty;
    
    [JsonInclude]
    public string Email { get; private set; } = string.Empty;


    // Används när kunder läses in från JSON.
    // Behövs eftersom vi har en annan konstruktor som tar emot namn och e-post.
    // JSON fyller sedan i kundens sparade uppgifter.
    [JsonConstructor]
    public Customer()
    {
    }


    //Används när vi skapar en ny kund.
    public Customer(string name, string email)
    {
        UpdateDetails(name, email);
    }
    
    //Validerar och uppfaterar kundens uppgifter
    public void UpdateDetails(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ange kundens namn eller företagsnamn.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Ange en e-postadress");

        name = name.Trim();
        email = email.Trim();

     // Kontrollerar e-postadressens format.
        if (!System.Net.Mail.MailAddress.TryCreate(email, out var address)
            || address.Address != email
            || !address.Host.Contains('.')
            || email.EndsWith('.'))
        {
            throw new ArgumentException("Ange en giltig e-postadress.");
        }

        Name = name;
        Email = email;
    }

}

