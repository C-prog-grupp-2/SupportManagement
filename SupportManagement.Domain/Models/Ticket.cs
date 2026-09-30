using SupportManagement.Domain.Enums;

namespace SupportManagement.Domain.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        public string TicketTitle { get; set; } = string.Empty;
        public string TicketDescription { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public TicketPriority Priority { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
