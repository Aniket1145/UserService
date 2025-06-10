# UserService Project
## Overview
This project is a .NET 8 implementation that demonstrates integration with an external API (reqres.in) to fetch and manage user data. It showcases best practices in building scalable and maintainable .NET applications with clean architecture principles.

## Project Structure
```
├── UserService/                     # Core Class Library
│   ├── Client/                      # API Client Implementation
│   ├── Models/                      # Data Models/DTOs
│   └── Services/                    # Business Logic Services
├── UserService.ConsoleApplication/  # Demo Console Application
│   └── Configuration/               # App Configuration
└── UserService.Tests/               # Unit Tests Project
```
## Build and Run
1. Build the solution:
```
dotnet build
```
2. Run the console application:
```
cd UserService.ConsoleApplication
dotnet run
```
## How It Works
The console application demonstrates the functionality of the UserService library by:

1. Fetching and displaying all users from the reqres.in API
2. Allowing users to input a specific user ID to fetch detailed information
3. Handling errors gracefully and displaying appropriate messages
## Design Decisions
### 1. Clean Architecture
- Separation of concerns with distinct layers (Client, Models, Services)
- Clear boundaries between external API communication and business logic
### 2. Dependency Injection
- Implemented DI for better:
  - Testability
  - Scalability
  - Maintainability
  - Loose coupling between components
### 3. Interface-Based Design
- Used interfaces (IReqResApiClient, IExternalUserService) for:
  - Abstraction of implementation details
  - Easier unit testing through mocking
  - Future extensibility
### 4. Configuration Management
- Externalized configuration in appsettings.json
- Flexible API endpoint configuration for different environments
### 5. Asynchronous Programming
- Proper implementation of async/await patterns
- Efficient handling of HTTP requests
### 6. Error Handling
- Comprehensive error handling for API communications
- Graceful handling of network issues and invalid responses
### 7. Testing
- Dedicated test project for unit tests
- Focus on testing business logic and API client behavior
