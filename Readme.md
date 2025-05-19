# WebjetMoviePortal

Webjet Movie Portal built using .Net 8 backend APIs and React frontend

## Technology Stack

### Backend (.NET 8)
- ASP.NET Core Web API
- HttpClient for thrid party Api integration
- Middleware for Header propegation
- In-memory distributed caching for backup in third party service outages
- Serilog for Logging
- Swagger UI for API Documentation

## Design Patterns Used

1. **Dependency Injection**
   - Loose coupling between components
   - Better testability

2. **CQRS services for segregation
   - Used Service Layer for Get calls to support query segregation so that it can be expanded for commands

3. Unit Test and Code coveragae
    - Implemented xUnit test case strategy as a sample

## Security Features

1. **Authentication & Authorization**
   - Access key provided

2. **OWASP Security**
   - Input validation
   - CORS configuration

## Performance Optimizations

**Backend**
   - Async/await for non-blocking operations
   - Proper model validation
   - Middleware to handle outgoing Http call header propegation

## Running the Application

Run the application:
   - appsettings.json - Repleace __Access_Token__ with valid access token  
   - dotnet run

## Things to improve

- Improve commenting on code
- Use Redis or SQL Caching for scaling
- Global error handling
- Use Key Vault to store the Access Key for security and have required RBACs enabled
- Fully functional CQRS pattern Mediatr
- Unit test all layers
- Opportunity to seperate Clean architecture with appropriate layers

## Assumptions

- In-Memory Cache Used for temporary caching purposes
- Data will be reset when the application restarts
- Secret is in the appsettings which has to be secured(using keyvault or database)
 