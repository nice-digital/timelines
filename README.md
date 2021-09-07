# Timelines
Provides reporting from Clickup timelines data

## What is it?
This service pulls data from the Click-up project management software via and API and stores the data in a SQL database that is accessible to the Planning tools reports.

## Stack
### Project structure
- NICE.Timelines.Common - contains models, constants and extension methods common to all projects
- NICE.Timelines.ConsoleApp - Entry point, calls the ClickUp API
- NICE.Timelines.DB - Converts and saves the ClickUp data to the database, contains models and migrations for Entity Framework
- NICE.Timelines.Test - low-level unit tests, run via xUnit, asserted with Shouldly.

### Technical stack
- [.NET Core 5](https://github.com/dotnet/core/tree/main/release-notes/5.0)
	- [Shouldly](https://github.com/shouldly/shouldly) for .NET assertions
	- [xUnit.net](https://xunit.github.io/) for .NET unit tests
	- [Moq](https://github.com/moq/moq4) for mocking in .NET
- [SQL Server](https://www.microsoft.com/en-gb/sql-server/sql-server-2017) as our database
    - [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore) as an ORM
    - [EF Core In-Memory Database Provider](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) for integration tests

## Set up
1. Install [SQL Server](https://www.microsoft.com/sql-server) and [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)
2. Clone the project at https://github.com/nice-digital/timelines.git
3. Open *NICE.Timelines.sln*
4. Ensure NICE.Timelines.ConsoleApp is set as the startup project
5. Update the values secrets.json file
6. Press F5 to run the project in debug mode

### Secrets.json
The application's uses appsettings.json to store configuration. However, since this is a public repository, confidential configuration information is stored in secrets.json
In order to run the application correctly (with it having access to a database and clickup api), you'll need to acquire (from another dev) or create a secrets.json file with the correct configuration information in. For more  information see: [https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio)

## Entity Framework Migrations
We use Code first Entity Framework migrations to update the consultations database  
 - add a new property to the relevent class in NICE.Timelines.DB > Models
 - in visual studio go to Tools > NuGet Package Manager > Package Manager Console
 - in the package manager console window run the command Add-Migration [give your migration a useful name] eg Add-Migration AddTaskCreationDate  
	This will create a new migrations script in NICE.Timelines.DB > Migrations
 - when NICE.Timelines.ConsoleApp is next run the changes in the migration script will be applied to SQL.   
 	A new column will be created in __EFMigrationHistory to flag that the migration has been run.__

## Good to know
For Developer notes see Confluence > Contacts Data Base / Planning Tool Home > Timelines Dev Notes