using SupportManagement.Domain.Models;
using SupportManagement.Domain.Enums;

namespace SupportManagement.Application.Interfaces
{
    public interface ITicketRegistrationService
    {
        Ticket RegisterTicket(string title, string description, Guid customerId, TicketPriority priority);
    }
}
