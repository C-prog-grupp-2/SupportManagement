using SupportManagement.Application.Interfaces;
using SupportManagement.Domain.Models;
using SupportManagement.Infrastructure.Helpers;
using System.Text.Json;

namespace SupportManagement.Infrastructure.Repositories;

public class JsonCustomerRepository : ICustomerRepository
{
    private readonly string _filePath = DataPath.CustomerFilePath; //Sökvägen till filen där kunderna sparas

    private readonly JsonSerializerOptions _options = new() 
    {
        WriteIndented = true, //Delar upp raderna, mer lättläsligt att läsa JSON-filen.
        PropertyNameCaseInsensitive= true 
    };
    
    public async Task<List<Customer>> GetAllAsync()
    {   
        if (!File.Exists(_filePath)) //Om kundfilen inte finns ännu finns inga sparade kunder.
            return [];

        var json = await File.ReadAllTextAsync(_filePath); //Läser innehållet i JSON-filen som text.

        var customers = JsonSerializer.Deserialize<List<Customer>>(json, _options);//Gör om JSON-texten till en lista med Customer-objekt.

        return customers ?? throw new JsonException("Kundfilen måste innehålla en lista.");
        
    }

    public async Task SaveAllAsync(IEnumerable<Customer> customers)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!); //Ser till att mappen där kundfilen ska sparas finns.
        
        var json = JsonSerializer.Serialize(customers, _options); //Gör om customers från C#-objekt till JSON-text.
        var temporaryPath = _filePath + ".tmp"; //Skapar en tillfällig filsökväg, ex customers.json.tmp.

        await File.WriteAllTextAsync(temporaryPath, json); //Skriv Json-texten till den tillfälliga filen, väntar tills det är klart.

        //Ersätter customer.json med den färdigskrivna tillfälliga filen.
        File.Move(temporaryPath, _filePath, overwrite: true); 
    }
}
