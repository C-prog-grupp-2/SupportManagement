
namespace SupportManagement.Domain.Models;

public class Customer
{
    public Guid Id { get; private set; } = Guid.NewGuid();//bara Customer-klassen får ändra Id:t o skapa den.

    public string Name { get; set; } =string.Empty;

    public string Email { get; set; } = string.Empty;

}
