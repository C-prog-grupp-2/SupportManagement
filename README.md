# SupportManagement

A Windows desktop application for managing customers and support tickets, built with C#, .NET 10, and WinUI. The user interface is in Swedish.

Customers and tickets are saved locally as JSON files, so no database or backend server needs to be configured.

## Features

- Create, view, edit, and delete customers, with validation of names and email addresses.
- Register support tickets linked to existing customers, with a title, description, and priority.
- View open tickets and counts of open, new, and high-priority tickets.
- View ticket details, customer information, creation time, and comments.
- Edit ticket titles and descriptions, change priorities and statuses, and assign a responsible technician.
- Add comments to tickets.
- Keep customer and ticket data between application sessions using local JSON storage.

### Ticket rules

| Setting | Available values |
| --- | --- |
| Priority | `Low`, `Normal`, `High` |
| Status | `New`, `InProgress`, `Resolved` |
| Technician choices in the UI | Anna, Erik, Johan |

New tickets start with the status `New`. A responsible technician must be assigned before a ticket can be marked `InProgress` or `Resolved`.

Resolved tickets remain saved but are excluded from the open-ticket overview. Customers with linked tickets cannot be deleted, including when those tickets are resolved.

## Technologies

- **C# and .NET 10** for application logic.
- **WinUI and Windows App SDK** for the desktop interface.
- **CommunityToolkit.Mvvm** for observable properties and commands using the MVVM pattern.
- **Microsoft.Extensions.DependencyInjection** for registering services, repositories, views, and view models.
- **System.Text.Json** for local data persistence.
- **xUnit and NSubstitute** for automated tests and test substitutes.

## Project structure

The solution separates the user interface, application services, domain models, and data storage into individual projects.

| Project | Responsibility |
| --- | --- |
| `SupportManagement.Presentation` | WinUI pages, view models, navigation, and dependency injection setup. |
| `SupportManagement.Application` | Customer and ticket services, plus service and repository interfaces. |
| `SupportManagement.Domain` | Customer, ticket, and comment models; priority and status enums; domain validation. |
| `SupportManagement.Infrastructure` | JSON repositories and local data paths. |
| `SupportManagement.Tests` | Unit tests and JSON repository integration tests. |

The solution file is `SupportManagement.slnx`.

## Requirements

- Windows. The presentation project declares Windows 10 version 1809 (build 17763) as its minimum platform version.
- The .NET 10 SDK.
- Visual Studio with support for .NET 10, `.slnx` solutions, and WinUI application development.
- Windows SDK and Windows App SDK development tooling for the presentation project. Its target framework is `net10.0-windows10.0.19041.0`.

NuGet package versions are defined in each project's `.csproj` file.

## Run in Visual Studio

1. Clone or download the repository.
2. Open `SupportManagement.slnx` in Visual Studio.
3. Allow Visual Studio to restore the NuGet packages.
4. Set `SupportManagement.Presentation` as the startup project.
5. Select **Debug** and the platform matching your machine, such as **x64**.
6. Select the **SupportManagement.Presentation (Package)** launch profile.
7. Press **F5** to build, deploy, and run the application.

If Visual Studio requests Windows Developer Mode for deployment, enable it and retry.

### Basic workflow

1. Open the customer section and register a customer with a name and email address.
2. Open the ticket overview and create a ticket for that customer.
3. Enter a title and description, select a priority, and save the ticket.
4. Open the ticket details and navigate to ticket management to edit it, assign a technician, update its status, or add comments.
5. Mark the ticket as resolved when the work is complete. It will disappear from the open-ticket overview.

## Data storage

The repositories build their paths from `Environment.SpecialFolder.LocalApplicationData`:

```text
%LOCALAPPDATA%\SupportManagement\customers.json
%LOCALAPPDATA%\SupportManagement\tickets.json
```

For a packaged deployment, Windows may redirect local application data into the package's per-user storage location.

The application starts with empty lists when the files do not exist. Directories and files are created when data is first saved. Saves write to a temporary `.tmp` file before replacing the destination JSON file.

Malformed JSON causes a read error rather than being treated as an empty list. The interface displays an error message when data cannot be loaded or saved.

## Tests

Run the tests from Visual Studio's **Test Explorer**, or run the following command from the repository root:

```powershell
dotnet test .\SupportManagement.Tests\SupportManagement.Tests.csproj
```

The test project references the application, domain, and infrastructure projects, so this command does not build the WinUI presentation project.

Existing tests cover customer creation and validation, saving customers through the service, ticket construction, technician requirements for status changes, JSON persistence, and handling invalid ticket JSON without modifying the file. Repository integration tests use temporary files rather than the application's normal data files.
