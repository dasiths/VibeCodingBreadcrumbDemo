# Car Rental API Setup

## Requirements

1. Create an ASP.NET Core API project using .NET 9 with the Minimal API approach
2. Set up the project structure according to the specification:
   - Solution name: CarRental
   - Project name: CarRental.Api
   - Location: src/backend/
3. Implement Swagger/OpenAPI endpoint according to the .NET 9 documentation
4. Create basic project structure with layers:
   - API Layer (CarRental.Api)
   - Data Layer (CarRental.Data)
   - Request Processing Layer (CarRental.RequestProcessing)
   - Model Layer (CarRental.Dtos)
5. Implement minimal endpoints for the vehicle entities
6. Use hardcoded responses initially (without database context)

## Additional comments from user

- No need to implement the database context yet
- Return hardcoded responses from the request processors
- Follow the .NET 9 OpenAPI document generation guidance from: https://devblogs.microsoft.com/dotnet/dotnet9-openapi/
- Move VehicleEndpoints to a separate file
- Proceed with implementing Make and Model endpoints
- Test API endpoints using .http file instead of Swagger UI

## Plan

### Phase 1: Project Setup
**Task 1.1: Create Solution and Project Structure**
- [x] Create the solution directory structure
- [x] Create the main solution file
- [x] Create the initial API project

**Task 1.2: Set Up Project References and Dependencies**
- [x] Create the Data project
- [x] Create the Request Processing project
- [x] Create the DTOs project
- [x] Set up project references between layers

### Phase 2: Implement Core Entities and DTOs
**Task 2.1: Define DTO Models**
- [x] Create basic DTO classes for Make
- [x] Create basic DTO classes for Model
- [x] Create basic DTO classes for Vehicle
- [x] Create basic DTO classes for Tag

**Task 2.2: Define Request/Response Models**
- [x] Create request models for vehicle operations
- [x] Create response models for vehicle operations
- [x] Create request models for make operations
- [x] Create response models for make operations
- [x] Create request models for model operations
- [x] Create response models for model operations

### Phase 3: Implement Request Processors
**Task 3.1: Create Request Processors with Hardcoded Data**
- [x] Implement GetVehicleRequestProcessor
- [x] Implement GetAllVehiclesRequestProcessor
- [x] Implement GetMakeRequestProcessor
- [x] Implement GetAllMakesRequestProcessor
- [x] Implement GetModelRequestProcessor
- [x] Implement GetModelsByMakeRequestProcessor

### Phase 4: Set Up API Layer
**Task 4.1: Configure API Project**
- [x] Set up Program.cs with required middleware and services
- [x] Configure Swagger/OpenAPI according to .NET 9 guidance
- [x] Register request processors

**Task 4.2: Implement API Endpoints**
- [x] Create extension methods to organize endpoints by entity
- [x] Implement vehicle endpoints
- [x] Implement make endpoints
- [x] Implement model endpoints
- [x] Move endpoint classes to separate files

**Task 4.3: Configure OpenAPI Documentation**
- [x] Set up proper OpenAPI document generation
- [x] Add meaningful endpoint descriptions and response types

### Phase 5: Test and Validate
**Task 5.1: Test API Functionality**
- [x] Build and run the solution
- [x] Create .http file for testing endpoints
- [x] Fix endpoint parameters to be optional when appropriate
- [x] Verify response data format with .http requests

## Decisions

- Using ASP.NET Core Minimal API approach for reduced boilerplate and improved performance
- Organizing the solution into four distinct layers according to the specification
- Using extension methods to organize endpoints by entity type
- Implementing hardcoded data responses for initial development without database context
- Following the Request Processor pattern to separate business logic from API endpoints
- Moving endpoint classes to separate files to improve code organization and maintainability
- Implementing Make and Model endpoints to complete the core entity relationships in the API
- Making pagination parameters optional in the Vehicle endpoints to improve usability
- Using .http file for API testing instead of Swagger UI for more streamlined testing

## Implementation Details

### Project Structure
We've implemented a layered architecture with the following components:
- **CarRental.Api**: Contains the API endpoints using ASP.NET Core Minimal API approach
- **CarRental.Data**: Reserved for future database implementation (currently empty)
- **CarRental.RequestProcessing**: Contains business logic with hardcoded data responses
- **CarRental.Dtos**: Contains data transfer objects for communication between layers

### DTO Models
Created DTO models based on the car rental entity relationship model:
- **MakeDto**: Represents vehicle manufacturers
- **ModelDto**: Represents vehicle models
- **VehicleDto**: Represents individual vehicles in the rental fleet
- **TagDto**: Represents labels for vehicle features and characteristics

### Request Processors
Implemented request processors with hardcoded sample data:
- **GetVehicleRequestProcessor**: Retrieves a specific vehicle by ID
- **GetAllVehiclesRequestProcessor**: Retrieves a paginated list of vehicles with search functionality
- **GetMakeRequestProcessor**: Retrieves a specific make by ID
- **GetAllMakesRequestProcessor**: Retrieves a list of all makes
- **GetModelRequestProcessor**: Retrieves a specific model by ID
- **GetModelsByMakeRequestProcessor**: Retrieves models filtered by make ID

### API Endpoints
Implemented minimal API endpoints using extension methods for organization:
- **GET /api/vehicles/{vehicleId}**: Retrieves a specific vehicle
- **GET /api/vehicles**: Retrieves a paginated list of vehicles with optional search
- **GET /api/makes/{makeId}**: Retrieves a specific make
- **GET /api/makes**: Retrieves a list of all makes
- **GET /api/models/{modelId}**: Retrieves a specific model
- **GET /api/makes/{makeId}/models**: Retrieves models for a specific make

### Optional Parameters
Modified the Vehicle endpoint implementation to make query parameters optional:
```csharp
app.MapGet("/api/vehicles", async (GetAllVehiclesRequestProcessor processor,
                                  CancellationToken cancellationToken,
                                  string? searchTerm = null, 
                                  int? pageNumber = 1, 
                                  int? pageSize = 10) =>
{
    // Ensure we have valid pagination values
    var validPageNumber = pageNumber ?? 1;
    var validPageSize = pageSize ?? 10;
    
    // Additional validation if values are provided but invalid
    if (validPageNumber <= 0) validPageNumber = 1;
    if (validPageSize <= 0) validPageSize = 10;
    
    var request = new GetAllVehiclesRequest 
    { 
        SearchTerm = searchTerm,
        PageNumber = validPageNumber,
        PageSize = validPageSize
    };
    
    var response = await processor.HandleAsync(request, cancellationToken);
    return Results.Ok(response);
})
```

### OpenAPI Configuration
Configured OpenAPI documentation using the .NET 9 approach:
- Added endpoint descriptions and response types
- Set up proper OpenAPI document generation for development environment

### HTTP Testing
Created a comprehensive .http file with test requests for all endpoints:
```http
@CarRental.Api_HostAddress = http://localhost:5054

### Vehicle Endpoints
GET {{CarRental.Api_HostAddress}}/api/vehicles
Accept: application/json

GET {{CarRental.Api_HostAddress}}/api/vehicles/1
Accept: application/json

GET {{CarRental.Api_HostAddress}}/api/vehicles?searchTerm=Honda&pageNumber=1&pageSize=10
Accept: application/json

### Make Endpoints
GET {{CarRental.Api_HostAddress}}/api/makes
Accept: application/json

GET {{CarRental.Api_HostAddress}}/api/makes/1
Accept: application/json

### Model Endpoints
GET {{CarRental.Api_HostAddress}}/api/models/1
Accept: application/json

GET {{CarRental.Api_HostAddress}}/api/makes/1/models
Accept: application/json
```

## Changes Made

1. Created the solution and project structure:
   - Created CarRental solution
   - Created CarRental.Api, CarRental.Data, CarRental.RequestProcessing, and CarRental.Dtos projects

2. Set up project references:
   - CarRental.Api references all other projects
   - CarRental.RequestProcessing references CarRental.Data and CarRental.Dtos

3. Implemented DTO models in CarRental.Dtos:
   - Created MakeDto.cs, ModelDto.cs, VehicleDto.cs, and TagDto.cs
   - Created request/response models in Requests and Responses folders for Vehicle, Make, and Model operations

4. Implemented request processors with hardcoded data:
   - Created request processors for Vehicle, Make, and Model operations with sample data

5. Configured the API layer:
   - Updated Program.cs with OpenAPI configuration
   - Registered request processors and set up dependency injection
   - Implemented vehicle, make, and model endpoints using extension methods
   - Moved endpoint classes to separate files in the Endpoints directory

6. Refactoring:
   - Moved endpoint classes to separate files for better code organization
   - Made pagination parameters optional in the Vehicle endpoints

7. Testing:
   - Updated the .http file to test all implemented endpoints
   - Successfully validated all endpoints return the expected data

## Before/After Comparison

Before:
- No API project structure
- No defined architecture or domain model implementation

After:
- Complete ASP.NET Core Minimal API project structure
- Layered architecture with separation of concerns
- Implemented endpoints for vehicles, makes, and models with hardcoded data
- Proper OpenAPI documentation configuration
- Working API with optional parameters for improved usability
- Better code organization with endpoint classes in separate files
- Fully implemented request processors with sample data for all core entities
- Comprehensive HTTP test file for easy API testing

## References

1. [ASP.NET Core Minimal API Architecture Specification](/workspaces/VibeCodingBreadcrumbDemo/.github/.copilot/specifications/application_architecture/aspnet-core-minimal-api.spec.md) - Used as the foundation for the API architecture.
2. [Car Rental Entity Relationship Model](/workspaces/VibeCodingBreadcrumbDemo/.github/.copilot/domain_knowledge/entities/car-rental-entities.md) - Used to understand the domain entities and their relationships.
3. [.NET 9 OpenAPI Document Generation](https://devblogs.microsoft.com/dotnet/dotnet9-openapi/) - Used for implementing Swagger/OpenAPI endpoints.