using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;
using SupportManagement.Infrastructure.Repositories;

namespace SupportManagement.Tests.IntegrationTests;

public class JsonRepositoryTests
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
            if (File.Exists(filePath + ".tmp"))
            {
                File.Delete(filePath + ".tmp");
            }
        }
    }

    [Fact]
    public async Task SaveAndLoadAsync_Ticket_PreservesData()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(), $"{Guid.NewGuid()}.json");

        var ticket = new Ticket("Test Ticket", "This is a test ticket.", Guid.NewGuid(), TicketPriority.Normal);

        try
        {
            var repository = new JsonTicketRepository(filePath);
            await repository.SaveAllAsync([ticket]);

            var loadRepository = new JsonTicketRepository(filePath);
            var tickets = await loadRepository.GetAllAsync();

            var savedTicket = Assert.Single(tickets);

            Assert.Equal(ticket.Id, savedTicket.Id);
            Assert.Equal(ticket.Title, savedTicket.Title);
            Assert.Equal(ticket.Description, savedTicket.Description);
            Assert.Equal(ticket.CustomerId, savedTicket.CustomerId);
            Assert.Equal(ticket.Priority, savedTicket.Priority);
            Assert.Equal(ticket.Status, savedTicket.Status);
            Assert.Equal(ticket.CreatedAtUtc, savedTicket.CreatedAtUtc);
        }

        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (File.Exists(filePath + ".tmp"))
            {
                File.Delete(filePath + ".tmp");
            }
        }
    }

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
        if (File.Exists(filePath + ".tmp"))
        {
            File.Delete(filePath + ".tmp");
        }
    }
}

