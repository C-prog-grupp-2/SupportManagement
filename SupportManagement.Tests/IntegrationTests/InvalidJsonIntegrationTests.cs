using SupportManagement.Infrastructure.Repositories;
using System.Text.Json;

namespace SupportManagement.Tests.IntegrationTests
{
    public class InvalidJsonIntegrationTests
    {
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
}
