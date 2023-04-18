# Project Title

A brief description of what this project does and who it's for

# Currency Exchange Microservice

This solution provides a currency exchange microservice for a bank, allowing clients to perform currency exchange transactions. It retrieves exchange rates from external APIs and stores transaction information.

### Features

Integration with external exchange rate providers (e.g., Fixer.io or ExchangeRatesAPI.io)
Retaining information about currency exchange trades carried out by clients
Ensuring exchange rates are never older than 30 minutes
Limiting each client to 10 currency exchange trades per hour

### Technologies

- C# (.NET 7)
- Entity Framework Core
- SQLite
- Caching
- xUnit and Moq for unit testing
- Logging

### Project Structure

The solution is organized into the following projects:

CurrencyExchange.Core: Contains the core entities, enums, interfaces, and services.
CurrencyExchange.Infrastructure: Contains the database context, entity configurations, repositories, and extensions.
CurrencyExchange.API: Contains the RESTful API, controllers, and DTOs.
CurrencyExchange.Tests: Contains the unit tests for the application.

### Getting Started

#### Prerequisites

- .NET 7 SDK
- SQLite

#### Configuration

- Update the appsettings.json file in the CurrencyExchange.API project with the appropriate settings for the database and exchange rate provider:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CurrencyExchangeDB.db"
  },
  "ExchangeRateProvider": {
    "BaseUrl": "https://api.apilayer.com",
    "ApiKey": "api_key_here",
    "CacheDurationInMinutes": 30
  }
  // ... other settings ...
}
```

### Running the Application

_To Apply The Migrations run following_

```
dotnet ef migrations add InitialCreate --project .\src\CurrencyExchange.Infrastructure\ -s .\src\CurrencyExchange.API\

dotnet ef database update --project .\src\CurrencyExchange.Infrastructure\ -s .\src\CurrencyExchange.API\
```

Navigate to the CurrencyExchange.API project folder:

```sh
cd CurrencyExchange.API
```

Run the application:

```sh
dotnet run
```

The RESTful API will be available at http://localhost:5000.

Running the Tests

Navigate to the CurrencyExchange.Tests project folder:

```sh
cd CurrencyExchange.Tests
```

Run the tests:

```sh
dotnet test
```

### API Endpoints

The following endpoints are available in the RESTful API:

```
GET /api/exchange/{id}: Retrieve a specific currency exchange transaction by its ID

POST /api/exchange: Perform a currency exchange transaction
Request body: { "clientId": int, "amount": decimal, "sourceCurrency": string, "targetCurrency": string }
```
