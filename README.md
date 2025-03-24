# ProductsMicroservice

Microservice for managing products in the e-shop.

## Tech stack
- .NET 8
- Microservice
- Clean architecture

## Structure
- Products.**Api** - REST API with usage of Minimal APIs
- Products.**Application** - Business logic
- Products.**Contracts** - DTOs, contracts
- Products.**Domain** - Domain models, entities
- Products.**Infrastructure** - External services
- Products.**Persistence** - 
- Products.**Tests** - Unit/integration tests

## Running the API
```
dotnet build
dotnet run --project Products.Api
```