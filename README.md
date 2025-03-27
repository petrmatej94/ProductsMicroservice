# ProductsMicroservice

This is REST API microservice for managing products in the e-shop.

## Features
- CRU(D) operations for products
- Stock management (including partial stock updates)
- SQLite as default database
- Versioned API (`v1`, `v2`)
- In-memory message queue for asynchronous processing in `v2`
- Exception handling middleware
- Follows Clean Architecture (Domain, Application, Infrastructure, Persistence, API layers)
- Covered by Unit tests

## Prerequisites
Make sure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)

## Tech stack
- .NET 8
- ASP.NET Core Web API (controller based)
- Entity Framework Core + SQLite
- Swagger (OpenAPI)
- FluentValidation
- InMemory queue

## Running the API
```bash
git clone https://github.com/petrmatej94/ProductsMicroservice.git
cd ProductsMicroservice

dotnet restore

dotnet ef database update --project Products.Persistence --startup-project Products.Api

dotnet build
dotnet run --project Products.Api
```

## The API will be available at
```
http://localhost:5046/swagger/index.html
OR
https://localhost:7240/swagger/index.html
```

## Running unit tests
```
dotnet test Products.Test
```