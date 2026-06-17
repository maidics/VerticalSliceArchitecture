# ASP.NET Core Vertical Slice Architecture Template

This template provides a lightweight starting point for ASP.NET solutions. While inspired by [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/tree/main), this project focuses strictly on Vertical Slice Architecture.

## Install the template

```
dotnet new install Vertical.Slice.Architecture
```

## Create a new solution

```
dotnet new vsa-sln -n [SolutionName]
```

## Key Features
- **Target Framework**: .NET SDK 10.0.202.
- **No MediatR dependency**: Uses lightweight reflection at application startup to automatically discover and register IRequestHandler & IDomainEventHandler
- **Native Pipeline Filters**: Because MediatR was removed, MediatR PipelineBehaviors have been replaced with native IEndpointFilter implementations. Cross-cutting concerns (validation, logging, performance) are handled natively at the endpoint level.
- **Persistence**: Configured with EF Core and SQLite out of the box.
- **App Host & SPA (WIP)**: Uses .NET App Host to orchestrate the backend and a React SPA. Note: The React client integration is currently a work in progress.

### Technologies
- [ASP.NET Core 10](https://learn.microsoft.com/en-us/aspnet/core/overview?view=aspnetcore-10.0)
- [Aspire](https://aspire.dev/)
- [React 19](https://react.dev/)
- [EF Core 10](https://learn.microsoft.com/en-us/ef/core/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Scalar](https://scalar.com/)
- [NUnit](https://nunit.org/), [Shouldly](https://docs.shouldly.org/), [Moq](https://github.com/devlooped/moq) & [Respawn](https://github.com/jbogard/Respawn)

## Structure
Code is grouped by feature rather than technical layer. Everything required to execute a specific feature lives in a single folder inside the Features directory.

```
ClientApp/ # React app
Common/
└──Pipeline/  # Native endpoint filters replacing MediatR behaviors
    ├── ExceptionHandler.cs
    ├── LoggingFilter.cs
    ├── PerformanceFilter.cs
    └── ValidationFilter.cs
    
Database/
Features/
└── Examples/
    ├── Commands/  # Commands, handlers and validators
    ├── Events/    # Domain events
    ├── Queries/   # Queries, handlers and validators
    ├── Example.cs # Entity
    ├── ExampleConfiguration.cs  # EF Core Entity Configuration
    ├── ExampleDto.cs            # DTO
    └── ExampleEndpoints.cs      # Minimal API Endpoints

Infrastructure/ # Services
```