# ProductsMicroservice

Microservice for managing products in the e-shop.

## Tech stack
- [.NET 8](https://dotnet.microsoft.com/en-us/download)
- REST API (Controller based)
- Microservice
- Clean architecture
- EF Core
- Swagger (OpenAPI)

## Structure
- Products.**Api** - REST API with usage of Controller based endpoints
- Products.**Application** - Business logic
- Products.**Contracts** - DTOs, contracts
- Products.**Domain** - Domain models, entities
- Products.**Infrastructure** - External services
- Products.**Persistence** - EF Core - DbContext
- Products.**Tests** - Unit and integration tests

## Running the API
```bash
dotnet build
dotnet run --project Products.Api
```

## API Versions
- v1
- v2