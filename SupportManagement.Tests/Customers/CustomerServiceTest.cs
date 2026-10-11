using NSubstitute;
using SupportManagement.Application.Interfaces;
using SupportManagement.Application.Services;
using SupportManagement.Domain.Models;
using Xunit;
namespace SupportManagement.Tests.Customers;

public class CustomerServiceTest
{
    [Fact]
    //Kontroll att CustomerService skickar rätt kunduppgifter till repositoryt för att spara
    public async Task CreateAsync_ValidCustomer_SavesCustomer()
    {
        //Arrange
        var customerRepository = Substitute.For<ICustomerRepository>();
        var ticketRepository = Substitute.For<ITicketRepository>();

        customerRepository.GetAllAsync().Returns([]);
        var service = new CustomerService(customerRepository, ticketRepository);
       
        string name = "Anna";
        string email = "anna@test.se";

        //Act
        await service.CreateAsync(name, email);

        //Assert - Kontrollera att SaveAllAsync anropats exakt 1 gång.
        // och att kundlistan innehåller exakt en kund med rätt namn & e-post
        await customerRepository.Received(1).SaveAllAsync(
            Arg.Is<IEnumerable<Customer>>(customers =>
            customers.Count()==1 && customers.Any(customer => customer.Name == name && customer.Email == email)));
    }
}
