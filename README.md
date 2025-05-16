# ServiceLog

ServiceLog is a web application for managing vehicle service records. It allows users to register, authenticate, add and manage vehicles, log service records, transfer vehicle ownership, and receive automatic email notifications about upcoming services. The application also supports uploading and storing vehicle-related images.

## Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- SQL Server (Docker)
- JWT Authentication
- Docker & Docker Compose
- Swagger / OpenAPI

## Features

- User registration and JWT-based authentication
- Role-based access
- Vehicle management with image upload support
- Service record logging per vehicle
- Ownership transfer of vehicles between users
- Automatic email notifications for upcoming service dates

## Getting Started

### Requirements

- .NET 8 SDK
- Docker & Docker Compose
- Visual Studio / VS Code / Rider

### Clone the Repository

git clone https://github.com/Burglak/ServiceLog.git
cd ServiceLog

### Restore Dependencies

dotnet restore

### Start SQL Server via Docker

docker-compose up

### Apply Migrations

update-database

### Run the Application

dotnet run




