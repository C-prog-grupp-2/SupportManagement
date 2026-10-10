namespace SupportManagement.Tests.Customers;

using SupportManagement.Domain.Models;
using Xunit;

public class CustomerTests
{

    [Fact]
    public void Constructor_ValidName_SetsCustomerName()
    {
        //Arrange - Förbereder namn & e-post. 
        string name = "Anna";
        string email = "anna@test.se";


        //Act - Skapa en kund
        var customer = new Customer(name, email);

        //Assert - kontrollera att namnet blev rätt
        Assert.Equal("Anna", customer.Name);

    }

    [Fact]
    public void Constructor_EmptyName_ThrowsArgumentException()
    {
        //Arrange 
        string name = "";
        string email = "anna@test.se";

        //Act & Assert - Kontrollera att ett ArgumentException kastas
        Assert.Throws<ArgumentException>(() => new Customer(name, email));

    }

    [Fact]
    public void Constructor_EmptyEmail_ThrowsArgumentException()
    {
        //Arrange
        string name = "Anna";
        string email = "";

        //Act & Assert:
        Assert.Throws<ArgumentException>(() => new Customer(name, email));
    }
}
