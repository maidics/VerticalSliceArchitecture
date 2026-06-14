# ASP.NET Core Vertical Slice Architecture Template

This template provides a lightweight starting point for ASP.NET solutions. While inspired by [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/tree/main), this project focuses strictly on Vertical Slice Architecture.

## Key Features
- **Target Framework**: .NET SDK 10.0.202.
- **No MediatR dependency**: Uses lightweight reflection at application startup to automatically discover and register IRequestHandler implementations and map Minimal API endpoint groups.
- **Native Pipeline Filters**: Because MediatR was removed, MediatR PipelineBehaviors have been replaced with ASP.NET Core's native IEndpointFilter. Cross-cutting concerns (validation, logging, performance) are handled natively at the endpoint level.
- **Persistence**: Configured with EF Core and SQLite out of the box.
- **App Host & SPA (WIP)**: Uses .NET App Host to orchestrate the backend and a React SPA (located in the ClientApp folder). Note: The React client integration is currently a work in progress.

## Project Structure
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