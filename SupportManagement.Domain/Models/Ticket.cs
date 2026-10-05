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

    public TicketStatus Status { get; private set; } = TicketStatus.New;

    public List<Comment> Comments { get; set; } = [];

    
    [JsonInclude]
    public string? ResponsibleTechnician { get; private set; }

    public void AssignTechnician(string technician)
    {
        if (string.IsNullOrWhiteSpace(technician))
            throw new ArgumentException("Ange en tekniker.", nameof(technician));

        ResponsibleTechnician = technician.Trim();
    }

    // Ändrar prioriteten på ett ärende.
    public void ChangePriority(TicketPriority priority)
    {
        if (!Enum.IsDefined(priority))
            throw new ArgumentException("Ange en giltig prioritet.", nameof(priority));

        Priority = priority;
    }

    // Ändrar statusen på ett ärende.
    public void ChangeStatus(TicketStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentException("Ange en giltig status.", nameof(status));

        // Pågående och lösta ärenden måste ha en ansvarig tekniker.
        if ((status == TicketStatus.InProgress || status == TicketStatus.Resolved)
            && string.IsNullOrWhiteSpace(ResponsibleTechnician))
        {
            throw new InvalidOperationException(
                "Ärendet måste ha en ansvarig tekniker för denna status.");
        }

        Status = status;
    }

    // Lägger till en kommentar på ett ärende.
    public void AddComment(string commentText)
    {
        if (string.IsNullOrWhiteSpace(commentText))
            throw new ArgumentException(
                "Kommentaren får inte vara tom.",
                nameof(commentText));

        Comments.Add(new Comment
        {
            CommentContent = commentText.Trim()
        });
    }
}
