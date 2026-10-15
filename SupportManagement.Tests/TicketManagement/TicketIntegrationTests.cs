
using SupportManagement.Domain.Enums;
using SupportManagement.Domain.Models;
using SupportManagement.Infrastructure.Repositories;
using System.Text.Json;

namespace SupportManagement.Tests.IntegrationTests;

public class TicketIntegrationTests
{
    [Fact]
    public async Task SaveTicket_WithTechnicianStatusAndComment_ShouldPersist()
    {
        // Arrange: Skapa en unik sökväg till en tillfällig JSON-fil.
        var filePath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid() + ".json");

        try
        {
            // Skapa repository med testfilen.
            var repository = new JsonTicketRepository(filePath);

            // Skapa ett nytt ärende.
            var ticket = new Ticket();

            ticket.UpdateDetails("Datorproblem", "Datorn startar inte.");
            ticket.CustomerId = Guid.NewGuid();
            ticket.ChangePriority(TicketPriority.Normal);

            // Tilldela tekniker, ändra status och lägg till kommentar.
            ticket.AssignTechnician("Anna");
            ticket.ChangeStatus(TicketStatus.InProgress);
            ticket.AddComment("Ärendet undersöks.");

            // Act: Spara ärendet i JSON-filen.
            await repository.SaveAllAsync(new[] { ticket });

            // Läs tillbaka ärendet med ett nytt repository.
            var newRepository = new JsonTicketRepository(filePath);
            var loadedTickets = await newRepository.GetAllAsync();

            // Assert: Kontrollera att informationen finns kvar.
            var loadedTicket = Assert.Single(loadedTickets);

            Assert.Equal(ticket.Id, loadedTicket.Id);
            Assert.Equal(ticket.CustomerId, loadedTicket.CustomerId);
            Assert.Equal("Anna", loadedTicket.ResponsibleTechnician);
            Assert.Equal(TicketStatus.InProgress, loadedTicket.Status);

            var comment = Assert.Single(loadedTicket.Comments);
            Assert.Equal("Ärendet undersöks.", comment.CommentContent);
        }
        finally
        {
            // Ta bort testfilen efter testet.
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    [Fact]
    public async Task GetAllAsync_InvalidJson_ShouldThrowAndPreserveFile()
    {
        // Arrange: Skapa en tillfällig fil med felaktig JSON.
        var filePath = Path.Combine(
            Path.GetTempPath(),
            Guid.NewGuid() + ".json");

        var invalidJson = "detta är inte giltig JSON";

        try
        {
            // Skriv felaktig JSON till testfilen.
            await File.WriteAllTextAsync(filePath, invalidJson);

            var repository = new JsonTicketRepository(filePath);

            // Act & Assert: Kontrollera att ett JSON-fel uppstår.
            await Assert.ThrowsAsync<JsonException>(
                () => repository.GetAllAsync());

            // Kontrollera att filens innehåll inte har ändrats.
            var fileContent = await File.ReadAllTextAsync(filePath);
            Assert.Equal(invalidJson, fileContent);
        }
        finally
        {
            // Ta bort testfilen efter testet.
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
