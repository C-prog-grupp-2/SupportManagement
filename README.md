# SupportManagement – Super Support

## Purpose

The purpose of Super Support is to provide a desktop application for managing customers and support tickets in one place. The application supports the handling of support cases from registration to resolution, allowing users to register customers, create and update tickets, assign technicians, and document progress through comments.

The application is built with C#, .NET 10 and WinUI, with a Swedish-language user interface. Customer and ticket data is stored locally in JSON files, without requiring a database or backend server.

## Team and Work Distribution

- **Gabriela Mora:** Project planning and coordination, shared application setup, customer management, including UI, services, validation, JSON persistence and related automated tests.
- **Alexander Giertz:** Ticket registration,ticket JSON repository, ticket overview, related UI and view models, navigation, MainWindow, automated tests and contributions to shared application functionality.
- **Maximilian Persson:** Ticket management, related UI and view models, ticket editing, technician assignment, status and priority updates, comments, validation and automated tests.
- **Fourth group member:** No completed contribution to the project.

These were the main areas of responsibility. The team also contributed to shared functionality, services, integration and testing across the application.

The development history and contributions can be found in the [GitHub pull request history](https://github.com/C-prog-grupp-2/SupportManagement/pulls?q=is%3Apr+state%3Aclosed).

## Features and Business Rules

- Register, view, update and delete customers, with validation of names and email addresses.
- Create support tickets linked to existing customers.
- View open tickets and counts of open, new and high-priority tickets.
- View ticket details, customer information, creation time and comments.
- Edit ticket titles and descriptions, change priority and status, and assign technicians.
- Add comments to support tickets.
- Save customer and ticket information between application sessions using JSON.

| Setting | Available values |
|---|---|
| Priority | `Low`, `Normal`, `High` |
| Status | `New`, `InProgress`, `Resolved` |
| Technicians | Anna, Erik, Johan |

New tickets start with the status `New`. A technician must be assigned before a ticket can be changed to `InProgress` or `Resolved`.

Resolved tickets remain stored in JSON but are automatically excluded from the ticket overview. Customers with linked tickets cannot be deleted, including when those tickets are resolved.

## Project Structure and Architecture

The solution contains four application projects and one separate test project.

| Project | Responsibility |
|---|---|
| `SupportManagement.Presentation` | WinUI pages, view models, navigation and dependency injection setup |
| `SupportManagement.Application` | Customer and ticket services, service interfaces and repository interfaces |
| `SupportManagement.Domain` | Customer, ticket and comment models, enums and business rules |
| `SupportManagement.Infrastructure` | JSON repositories, file handling and local data paths |
| `SupportManagement.Tests` | Unit tests and JSON repository integration tests |

The presentation layer sends user actions to application services. The services apply domain rules and use repository interfaces implemented by the infrastructure layer. Data is read from or saved to JSON files, and the results are returned to the interface.

The application uses CommunityToolkit.Mvvm for MVVM, Microsoft.Extensions.DependencyInjection for dependency injection, System.Text.Json for JSON storage, and xUnit with NSubstitute for automated testing.

The solution file is `SupportManagement.slnx`.

## Build and Run

### Requirements

- Windows
- .NET 10 SDK
- Visual Studio with support for .NET 10, `.slnx` solutions and WinUI
- Windows SDK and Windows App SDK development tooling

The presentation project targets `net10.0-windows10.0.19041.0` and declares Windows 10 version 1809 (build 17763) as its minimum platform version.

### Run in Visual Studio

1. Clone or download the repository.
2. Open `SupportManagement.slnx` in Visual Studio.
3. Allow Visual Studio to restore NuGet packages.
4. Set `SupportManagement.Presentation` as the startup project.
5. Select **Debug** and the appropriate platform, such as **x64**.
6. Select the **SupportManagement.Presentation (Package)** launch profile.
7. Press **F5** to build, deploy and run the application.

If Visual Studio requests Windows Developer Mode, enable it and retry.

### Run Automated Tests

Run the tests through Visual Studio's **Test Explorer**, or execute the following command from the repository root:

```powershell
dotnet test .\SupportManagement.Tests\SupportManagement.Tests.csproj
```

The tests cover customer services and validation, ticket rules, JSON persistence and invalid JSON handling. Repository integration tests use temporary files rather than the application's normal data files.

This command runs the test project without building the WinUI presentation project.

## JSON Storage and Sample Data

The application stores data in the current user's local application data folder:

```text
%LOCALAPPDATA%\SupportManagement\customers.json
%LOCALAPPDATA%\SupportManagement\tickets.json
```

Packaged Windows applications may redirect these paths to package-specific local storage.

If the files do not exist, the application starts with empty lists. Directories and files are created when data is first saved. Saves use a temporary `.tmp` file before replacing the destination file.

Malformed JSON produces a read error displayed in the interface instead of silently replacing the data.

### Using Your Own Sample Data

The simplest way to create example data is to register customers and tickets directly in the application.

To use existing JSON files:

1. Close the application.
2. Back up any existing customer and ticket files.
3. Create or edit `customers.json` and `tickets.json` in the application's local data folder.
4. Restart the application.

A ticket's `CustomerId` must match an existing customer's `Id`.

The JSON format uses PascalCase property names and numeric enum values. Priorities are `0` = `Low`, `1` = `Normal`, `2` = `High`. Statuses are `0` = `New`, `1` = `InProgress`, `2` = `Resolved`.

Example `customers.json`:

```json
[
  {
    "Id": "c2f970a4-454e-4b8c-9b56-e333fd5ea137",
    "Name": "Example Customer Ltd",
    "Email": "contact@example.com"
  }
]
```

Example `tickets.json`:

```json
[
  {
    "Id": "01a940c5-5bdd-4d29-ae53-41ee3bf989ad",
    "Title": "Unable to sign in",
    "Description": "Sign-in fails after the password was changed.",
    "CustomerId": "c2f970a4-454e-4b8c-9b56-e333fd5ea137",
    "Priority": 1,
    "CreatedAtUtc": "2026-10-18T10:00:00Z",
    "Status": 0,
    "Comments": [],
    "ResponsibleTechnician": null
  }
]
```

The application does not provide a setting for an alternative data directory.

## Requirements Checklist and Known Limitations

| Requirement | Status |
|---|---|
| Customer registration and management | Completed |
| Customer validation | Completed |
| Ticket registration linked to customers | Completed |
| Ticket overview and details | Completed |
| Ticket updates, technician assignment and comments | Completed |
| Navigation between application pages | Completed |
| JSON storage and persistence | Completed |
| Automated unit and integration tests | Completed |
| Ticket search by title or customer name | Not implemented |
| Ticket filtering by status | Not implemented |
| Combined search and status filtering | Not implemented |

**Known limitations:**

- Ticket search and status filtering have not been implemented.
- Resolved tickets are hidden from the overview, with no option to display or review them through the overview. They remain saved in JSON.
- The application uses local JSON storage rather than a shared database or server.
- The application does not provide user authentication or automatic data synchronization.
- Technician selection is limited to Anna, Erik and Johan.
- Invalid JSON files must be corrected or restored from a backup.

## Manual Verification

The following manual tests were completed successfully:

- [x] Register a customer and verify that the customer appears in the customer list.
- [x] Update customer information and verify that changes are saved.
- [x] Create a ticket linked to an existing customer and verify that it appears in the overview.
- [x] Verify that a ticket cannot be moved to `InProgress` without an assigned technician.
- [x] Assign a technician, update ticket priority and status, and add a comment.
- [x] Restart the application and verify that customer and ticket data remains saved.
- [x] Verify that invalid customer input produces an appropriate error message.
- [x] Resolve a ticket and verify that it disappears from the open-ticket overview.
- [x] Verify that customers with linked tickets cannot be deleted.

All listed manual checks passed.

## AI Usage

AI tools, including ChatGPT, were used as support during development to explain programming concepts, assist with debugging, discuss architectural approaches and suggest possible solutions.

AI was also used to assist with structuring and reviewing the project documentation against the assignment requirements.

AI-generated suggestions were evaluated against the project requirements and reviewed by the team. Implemented code was verified through automated tests, manual testing and code reviews where applicable.

The team remained responsible for understanding the code, making implementation decisions, integrating features and verifying the final application.