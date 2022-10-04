# MudBlazorTemplate
Blazor Server Template using MudBlazor

### Built With
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [MudBlazor](https://mudblazor.com)

### Content

- [Main branch](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/main) includes only the Dark Theme
- [Sql branch](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/sql) includes EF Core and SQL Server setup
- [User management](https://github.com/TheDayIsMyEnemy/MudBlazorTemplate/tree/user-management) branch includes all the features

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
