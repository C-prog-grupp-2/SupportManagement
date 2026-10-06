using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using SupportManagement.Infrastructure.Helpers;
using System.Text.Json;

namespace SupportManagement.Infrastructure.Repositories;

public class JsonTicketRepository : ITicketRepository
{
    private readonly string _filePath = DataPath.TicketFilePath;

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<Ticket>> GetAllAsync()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = await File.ReadAllTextAsync(_filePath);

        var tickets = JsonSerializer.Deserialize<List<Ticket>>(json, _options);

        return tickets ?? throw new JsonException(
            "Ärendefilen måste innehålla en lista.");
    }

    public async Task SaveAllAsync(IEnumerable<Ticket> tickets)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var json = JsonSerializer.Serialize(tickets, _options);
        var temporaryPath = _filePath + ".tmp";
        await File.WriteAllTextAsync(temporaryPath, json);
        File.Move(temporaryPath, _filePath, overwrite: true);
    }
}
