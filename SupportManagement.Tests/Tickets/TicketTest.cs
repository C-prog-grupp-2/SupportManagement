using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;
using Xunit;

namespace SupportManagement.Tests.Tickets;

public class TicketTest
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        string title = "Test Ticket";
        string description = "This is a test ticket.";
        Guid customerId = Guid.NewGuid();
        TicketPriority priority = TicketPriority.Normal;
        // Act
        var ticket = new Ticket(title, description, customerId, priority);
        // Assert
        Assert.Equal(title, ticket.Title);
        Assert.Equal(description, ticket.Description);
        Assert.Equal(customerId, ticket.CustomerId);
        Assert.Equal(priority, ticket.Priority);

        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal(TicketStatus.New, ticket.Status);
        Assert.Empty(ticket.Comments);
        Assert.Null(ticket.ResponsibleTechnician);
    }

    [Fact]
    public void Constructor_EmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        string title = "";
        string description = "This is a test ticket.";
        Guid customerId = Guid.NewGuid();
        TicketPriority priority = TicketPriority.Normal;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Ticket(title, description, customerId, priority));
    }
}

