using SupportManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace SupportManagement.Domain.Models;

public class Ticket
{
    [JsonInclude]
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public TicketPriority Priority { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public TicketStatus Status { get; set; } = TicketStatus.New;

    public List<Comment> Comments { get; set; } = [];
}
