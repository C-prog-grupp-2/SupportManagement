
using System;
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using Xunit;

namespace SupportManagement.Tests.TicketManagement;

public class TicketStatusTests
{
    [Fact]
    public void ChangeStatus_ToInProgress_WithoutTechnician_ShouldThrow()
    {
        // Arrange: Skapa ett nytt ärende utan tekniker.
        var ticket = new Ticket();

        // Kontrollera att ärendet börjar med status New.
        Assert.Equal(TicketStatus.New, ticket.Status);

        // Act: Försök ändra status till InProgress.
        var exception = Assert.Throws<InvalidOperationException>(
            () => ticket.ChangeStatus(TicketStatus.InProgress));

        // Assert: Kontrollera att rätt felmeddelande visas.
        Assert.Equal(
            "Ärendet måste ha en ansvarig tekniker för denna status.",
            exception.Message);

        // Assert: Kontrollera att statusen fortfarande är New.
        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_ToResolved_WithoutTechnician_ShouldThrow()
    {
        // Arrange: Skapa ett nytt ärende utan tekniker.
        var ticket = new Ticket();

        // Kontrollera att ärendet börjar med status New.
        Assert.Equal(TicketStatus.New, ticket.Status);

        // Act: Försök ändra status till Resolved.
        var exception = Assert.Throws<InvalidOperationException>(
            () => ticket.ChangeStatus(TicketStatus.Resolved));

        // Assert: Kontrollera att rätt felmeddelande visas.
        Assert.Equal(
            "Ärendet måste ha en ansvarig tekniker för denna status.",
            exception.Message);

        // Assert: Kontrollera att statusen fortfarande är New.
        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_ToInProgress_WithTechnician_ShouldSucceed()
    {
        // Arrange: Skapa ett nytt ärende.
        var ticket = new Ticket();

        // Kontrollera att ärendet börjar med status New.
        Assert.Equal(TicketStatus.New, ticket.Status);

        // Tilldela en tekniker.
        ticket.AssignTechnician("Erik");

        // Kontrollera att teknikern har tilldelats.
        Assert.Equal("Erik", ticket.ResponsibleTechnician);

        // Act: Ändra ärendets status till InProgress.
        ticket.ChangeStatus(TicketStatus.InProgress);

        // Assert: Kontrollera att statusändringen lyckades.
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_ToResolved_WithTechnician_ShouldSucceed()
    {
        // Arrange: Skapa ett nytt ärende.
        var ticket = new Ticket();

        // Kontrollera att ärendet börjar med status New.
        Assert.Equal(TicketStatus.New, ticket.Status);

        // Tilldela en tekniker.
        ticket.AssignTechnician("Anna");

        // Kontrollera att teknikern har tilldelats.
        Assert.Equal("Anna", ticket.ResponsibleTechnician);

        // Act: Ändra ärendets status till Resolved.
        ticket.ChangeStatus(TicketStatus.Resolved);

        // Assert: Kontrollera att statusändringen lyckades.
        Assert.Equal(TicketStatus.Resolved, ticket.Status);
    }
}
