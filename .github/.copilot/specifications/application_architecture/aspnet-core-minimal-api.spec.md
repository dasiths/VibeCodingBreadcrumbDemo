# Specification: ASP.NET Core Minimal API Architecture

**Version:** 1.0

**Last Updated:** 2025-04-13

**Owner:** Dasith Wijes

## 1. Purpose & Scope

This specification defines the architecture and implementation guidelines for the Car Rental API using ASP.NET Core with the Minimal API approach. It covers project structure, layer responsibilities, and patterns to be used for developing a clean, maintainable, and scalable API.

## 2. Core Principles & Guidelines

### Project Structure

The application is organized into four distinct layers:

1. **API Layer** (`MyProject.Api`)
   * Handles HTTP requests and routes them to appropriate request processors
   * Implements Minimal API endpoints instead of controllers
   * Defines API routing, input validation, and response formatting
   * Dependencies: Data Layer and Request Processing Layer

2. **Data Layer** (`MyProject.Data`)
   * Manages database context, entity models, and data access logic
   * Implements Entity Framework Core for database operations
   * Defines domain entities with their relationships
   * No dependencies on other application layers

3. **Request Processing Layer** (`MyProject.RequestProcessing`)
   * Contains all business logic and orchestration
   * Implements request processors for handling use cases
   * Mediates between API and Data layers
   * Dependencies: Data Layer and DTOs

4. **Model Layer** (`MyProject.Dtos`)
   * Contains Data Transfer Objects (DTOs) for communication between layers
   * Defines data structures with minimal logic
   * No dependencies on other application layers

### Minimal API Implementation

* Use extension methods to organize endpoints by entity or feature
* Structure endpoints following RESTful conventions
* Implement appropriate HTTP status codes and responses
* Use parameter binding with model validation
* Utilize dependency injection for request processors

### Request Processor Pattern

* Each API endpoint should delegate to a request processor
* Request processors handle all business logic
* Implement processors based on clear request/response contracts
* Follow Single Responsibility Principle
* Enable unit testing of business logic in isolation

### Data Access

* Use Entity Framework Core as the ORM
* Implement in-memory database for development/testing
* Define relationships in the OnModelCreating method
* Implement seed data method for development

## 3. Rationale & Context

The Minimal API approach was chosen over traditional controller-based architecture for:

* **Reduced Boilerplate**: Less code to write and maintain
* **Improved Performance**: More efficient request handling
* **Readability**: More concise and readable code
* **Modern Approach**: Leverages latest ASP.NET Core features

The layered architecture provides:

* **Separation of Concerns**: Each layer has a distinct responsibility
* **Testability**: Business logic is isolated and testable
* **Maintainability**: Changes in one layer don't affect others
* **Scalability**: Components can evolve independently

## 4. Examples

### Good Example (Do)

#### Organizing Endpoints with Extension Methods

```csharp
// Program.cs
var app = builder.Build();
// Configure app...

app.MapOrderEndpoints();
app.MapVehicleEndpoints();

app.Run();

// OrderEndpoints.cs
public static class OrderEndpointsExtensions
{
    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("api/orders/{orderId}", async (int orderId, GetOrderRequestProcessor processor) =>
        {
            var result = await processor.HandleAsync(new GetOrderRequest { OrderId = orderId });
            return result.Result != null ? Results.Ok(result.Result) : Results.NotFound();
        })
        .WithName("GetOrder")
        .WithOpenApi();
        
        // More endpoints...
        
        return app;
    }
}
```

#### Request Processor Implementation

```csharp
public class GetOrderRequestProcessor
{
    private readonly DemoDbContext _demoDbContext;

    public GetOrderRequestProcessor(DemoDbContext demoDbContext)
    {
        _demoDbContext = demoDbContext;
    }

    public async Task<GetOrderResponse> HandleAsync(GetOrderRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _demoDbContext.Orders
            .Include(o => o.Products)
            .Where(o => o.OrderId == request.OrderId)
            .Select(o => new OrderDto()
            {
                Customer = o.Customer,
                OrderId = o.OrderId,
                Products = o.Products.Select(p => new ProductDto()
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new GetOrderResponse()
        {
            Result = result
        };
    }
}
```

### Bad Example (Don't / Avoid)

#### Mixing Business Logic in API Endpoints

```csharp
// Avoid this approach
app.MapGet("api/orders/{orderId}", async (int orderId, DemoDbContext dbContext) =>
{
    // Business logic mixed with API endpoint handling
    var order = await dbContext.Orders
        .Include(o => o.Products)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        
    if (order == null)
        return Results.NotFound();
        
    // Transformation logic in API layer
    var orderDto = new OrderDto
    {
        OrderId = order.OrderId,
        Customer = order.Customer,
        Products = order.Products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            Name = p.Name
        }).ToList()
    };
    
    return Results.Ok(orderDto);
})
.WithName("GetOrder")
.WithOpenApi();
```

#### Using Domain Entities in API Responses

```csharp
// Avoid this approach
app.MapGet("api/orders/{orderId}", async (int orderId, DemoDbContext dbContext) =>
{
    var order = await dbContext.Orders.FindAsync(orderId);
    return order != null ? Results.Ok(order) : Results.NotFound(); // Returning domain entity directly
})
.WithName("GetOrder")
.WithOpenApi();
```

## 5. Related Specifications / Further Reading

- [Database Schema Specification](/home/dasith/repos/VibeCodingBreadcrumbDemo/.github/.copilot/specifications/database/main.spec.md)
- [Microsoft's ASP.NET Core Minimal APIs Documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

## 6. Keywords

Minimal API, ASP.NET Core, Request Processor, Entity Framework Core, Clean Architecture, Layered Architecture