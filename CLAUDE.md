# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build

# Run API (requires PostgreSQL running)
dotnet run --project source/CodesCampaigns.Api

# Run all tests
dotnet test

# Run a specific test project
dotnet test source/CodesCampaigns.Domain.Tests
dotnet test source/CodesCampaigns.Infrastructure.Tests
dotnet test source/CodesCampaigns.Api.Tests
dotnet test source/CodesCampaigns.Api.Tests.Integration

# Run a single test by name
dotnet test --filter "FullyQualifiedName~TestClassName"

# Add EF Core migration
dotnet ef migrations add <MigrationName> --project source/CodesCampaigns.Infrastructure --startup-project source/CodesCampaigns.Api

# Start PostgreSQL (required for local dev and EF migrations)
docker compose up -d
```

The default connection string (in `appsettings.json`) targets `localhost:5432` with database `mydb`, user `myuser`, password `mypassword` — matching the docker-compose postgres service.

## Architecture

This is a .NET 9 Clean Architecture API with four layers:

- **`CodesCampaigns.Domain`** — Pure domain model. Contains `Campaign` and `TopUp` entities, value objects (`CampaignId`, `TopUpCode`, `Money`, `CurrencyCode`), repository interfaces, and the `IClock` abstraction. No external dependencies.
- **`CodesCampaigns.Application`** — Use-case layer. Contains commands, queries, handlers, and the `GenerateTopUpBatchJob`. Uses `ICommandHandler<TCommand>` and `IQueryHandler<TQuery, TResponse>` interfaces (custom CQRS, no MediatR). Handlers are auto-registered via Scrutor assembly scanning in `Registry.cs`.
- **`CodesCampaigns.Infrastructure`** — EF Core + PostgreSQL implementation. Has its own `Campaign`/`TopUp` entity classes (separate from domain entities) and factory classes (`DomainCampaignFactory`, `CampaignEntityFactory`, etc.) to map between domain and infrastructure entities. Hangfire with PostgreSQL storage is configured here.
- **`CodesCampaigns.Api`** — ASP.NET Core Web API. Controllers inject `ICommandHandler`/`IQueryHandler` directly via action method parameter injection (not constructor injection). Uses API key authentication via `[ApiKey]` attribute (header: `X-API-KEY`, configured in `appsettings.json` under `Authentication:ApiKey`).

### Key Design Patterns

**CQRS without MediatR**: Commands and queries are dispatched by injecting the specific handler interface (`ICommandHandler<CreateCampaignCommand>`) directly into controller action parameters.

**Dual entity model**: Infrastructure maintains its own EF entity classes under `CodesCampaigns.Infrastructure/Entities/` separate from domain entities. Factories (`*EntityFactory`, `Domain*Factory`) handle bidirectional mapping.

**Background job batching**: `GenerateTopUpCodesCommand` is handled by enqueuing Hangfire background jobs in batches of 10 (`GenerateTopUpCodesCommandHandler` → `GenerateTopUpBatchJob`).

**`IClock` abstraction**: All time access goes through `IClock` (injected as `Clock` in production, `FakeClock` in tests) to allow deterministic testing.

### Test Projects

- **`CodesCampaigns.Domain.Tests`** — Unit tests for value objects.
- **`CodesCampaigns.Infrastructure.Tests`** — Unit tests for factory mapping classes.
- **`CodesCampaigns.Api.Tests`** — Unit tests for DTO factories.
- **`CodesCampaigns.Api.Tests.Integration`** — BDD integration tests using Reqnroll (SpecFlow successor) + Testcontainers. Spins up a real PostgreSQL container per feature via `FeatureHooks`. Uses `CustomWebApplicationFactory` which swaps in the Testcontainers connection string and uses Hangfire in-memory storage.

### Build Configuration

- `Directory.Build.props` applies globally: `TreatWarningsAsErrors=true`, `AnalysisMode=All`, `EnforceCodeStyleInBuild=true`, nullable enabled.
- `Directory.Packages.props` uses Central Package Management — add package versions here, not in individual `.csproj` files.
