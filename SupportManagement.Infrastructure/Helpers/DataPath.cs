using System.Net.NetworkInformation;

namespace SupportManagement.Infrastructure.Helpers;

public static class DataPath
{
    public static readonly string CustomerFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "SupportManagement",
    "customers.json"
    );

    public static readonly string TicketFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "SupportManagement",
    "tickets.json"
    );
}

//Ger sökvägen till den här användarens lokala AppData-mapp.