using SupportManagement.Domain.Models;
using SupportManagement.Infrastructure.Repositories;

namespace SupportManagement.Tests.Customers;

public class JsonCustomerRepositoryTests
{
    [Fact]
    public async Task SaveAndLoadAsync_Customer_PreservesData()
    {
        //ARRANGE:

        //Skapa en unik sökväg för testets tillfälliga JSON-fil:
        string filePath = Path.Combine(
            Path.GetTempPath(), $"{Guid.NewGuid()}.json");

        var customer = new Customer("Anna", "anna@test.se");

        try
        {
            //ACT - spara kunden och läs tillbaka den.
            
            var repository = new JsonCustomerRepository(filePath); //Skapa ett repository med testfilens sökväg
            await repository.SaveAllAsync([customer]);   //Spara kunden till JSON-filen

            var loadRepository = new JsonCustomerRepository(filePath); //Skapa en ny repository instans som läser samma Jsonfil

            var customers = await loadRepository.GetAllAsync();   //Läs tillbaka kunderna från filen


            //ASSERT
            var savedCustomer = Assert.Single(customers); //kontrollerar att exakt en kund läses tillbaka.

           // Kontrollera att kundens uppgifter är oförändrade
            Assert.Equal(customer.Id, savedCustomer.Id);
            Assert.Equal(customer.Name, savedCustomer.Name);
            Assert.Equal(customer.Email, savedCustomer.Email);
        }

        finally //körs även om kontrollen ovan misslyckas.
        {

            if (File.Exists(filePath))
            {
                File.Delete(filePath); //Tar bort den tillfälliga Json-filen
            }
        }
    }
}
