using System.Net.NetworkInformation;

namespace SupportManagement.Infrastructure.Helpers;

public static class DataPath
{
    public static readonly string CustomerFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "SupportManagement",
    "customers.json"
    );
}

//Ger sökvägen till den här användarens lokala AppData-mapp.