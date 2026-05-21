# EventHub

EventHub is an ASP.NET Core MVC event and workshop management application prepared for a web programming course project. The solution follows a layered structure with separate `DAL`, `BLL`, and `EventHub.Web` projects.

## Scope

The application is being aligned to the course requirement `Etkinlik & Workshop Yonetim Sistemi (EventHub)`.

Current functional scope:

- Users can register, sign in, browse events, join events, cancel their own bookings, and review their booking list.
- Admins can create, edit, delete, and review events.
- Admins can review attendee lists for each event.
- Event capacity is enforced during booking.

## Architecture

- `EventHub.Web`: MVC UI, controllers, Razor views, Identity UI
- `BLL`: business services, mail helpers, attachment helpers
- `DAL`: Entity Framework Core context, entities, repositories, migrations, seed data

## Technology Stack

- .NET 10
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQLite
- Razor Views

## Solution Layout

```text
EventHub.sln
BLL/
DAL/
EventHub.Web/
```

## Setup

1. Restore packages:

```bash
dotnet restore EventHub.sln
```

2. Review the development settings in [appsettings.Development.json](/Users/yasir.arslan/Desktop/Event-Managment-System/EventHub.Web/appsettings.Development.json:1).
   The default configuration already points to the local SQLite database file `EventHub.Web/eventhub.db`.

3. Apply migrations:

```bash
dotnet ef database update --project DAL/DAL.csproj --startup-project EventHub.Web/EventHub.Web.csproj
```

4. Run the web application:

```bash
dotnet run --project EventHub.Web/EventHub.Web.csproj
```

## Default Seed Data

The seed routine creates:

- default sample events
- `Admin` and `User` roles
- an admin user with email `admin@eventhub.local`
- admin password `Admin123!`

## Local Verification

Run the web project:

```bash
dotnet run --project EventHub.Web/EventHub.Web.csproj --launch-profile http
```

Default local URL:

- `http://localhost:5079`

Basic checks:

- open the login page
- sign in with `admin@eventhub.local / Admin123!`
- verify admin event management and attendee listing
- register a regular user and verify event booking flow

## Tests

Run unit tests with:

```bash
dotnet test EventHub.Tests/EventHub.Tests.csproj
```

## Notes

- `DAL` and `BLL` build successfully in the local environment.
- If `dotnet build` for `EventHub.Web` hangs in a sandboxed runner, prefer verifying `dotnet run` or running build/test commands with single-node MSBuild settings.
