# MudBlazorTemplate
Blazor Server Template using MudBlazor

### Built With
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0#blazor-server)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [MudBlazor](https://mudblazor.com)

### Branches Content

- [Basic](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/basic) includes MudBlazor setup and Dark Theme
- [Sql](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/sql) includes EF Core and SQL Server setup
- [Main](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/main) includes all the features

### Features

- Dark Theme
- Admin Role Seeding
- Admin User Seeding
- User Management Page

### Installation

1. [Download and install the .NET Core SDK](https://dotnet.microsoft.com/download)
2. [Download and install SQL Server](https://go.microsoft.com/fwlink/p/?linkid=866662)

### How to run locally

1. Open a command prompt in the project folder.
2. `dotnet tool install --global dotnet-ef`
3. `dotnet ef migrations add <NAME OF MIGRATION> --output-dir ./Data/Migrations`
4. `dotnet run`
5. Open your browser to: `https://localhost:5001`
