# ASP.NET Core API Specification

## Requirements

1. Create a specification for an ASP.NET Core API using the Minimal API approach
2. Follow the provided project structure:
   - API Layer (`MyProject.Api`)
   - Data Layer (`MyProject.Data`)
   - Request Processing Layer (`MyProject.RequestProcessing`)
   - Model Layer (`MyProject.Dtos`)
3. Implement CRUD operations and endpoints for managing entities
4. Organize code according to best practices for Minimal APIs
5. Define clear dependencies between projects
6. Include implementation details for key components and files
7. Document endpoint structure and request processing flow

## Additional comments from user

- User wants to use ASP.NET Core with Minimal APIs approach
- The project structure includes four main layers with distinct responsibilities
- The sample project includes Order and Product entities with a many-to-many relationship
- In-memory database will be used for development/demo purposes

## Plan

### Phase 1: Define Domain Models and Entities
**Task 1.1: Create Product Entity**
- [x] Define Product class with required properties
- [x] Include relationships with other entities

**Task 1.2: Create Order Entity**
- [x] Define Order class with required properties
- [x] Define relationships with Product entities

### Phase 2: Setup Data Access Layer
**Task 2.1: Create DbContext**
- [x] Define DemoDbContext with DbSets
- [x] Configure entity relationships
- [x] Implement seed data method for development

### Phase 3: Define DTOs (Data Transfer Objects)
**Task 3.1: Create ProductDto**
- [x] Define ProductDto class with properties needed for API responses

**Task 3.2: Create OrderDto**
- [x] Define OrderDto class with properties
- [x] Include relationship with ProductDto

### Phase 4: Implement Request Processing
**Task 4.1: Define Request/Response Objects**
- [x] Create request objects for CRUD operations
- [x] Create response objects with appropriate result structures

**Task 4.2: Create Request Processors**
- [x] Implement GetOrderRequestProcessor
- [x] Implement GetProductRequestProcessor
- [x] Implement CreateOrderRequestProcessor
- [x] Implement UpdateOrderRequestProcessor
- [x] Implement DeleteOrderRequestProcessor

### Phase 5: Configure API Layer
**Task 5.1: Setup Program.cs**
- [x] Configure services
- [x] Register request processors
- [x] Configure database context

**Task 5.2: Define API Endpoints**
- [x] Implement GET endpoints for retrieving data
- [x] Implement POST endpoints for creating resources
- [x] Implement PUT endpoints for updating resources
- [x] Implement DELETE endpoints for removing resources

**Task 5.3: Organize Endpoints**
- [x] Create extension methods for endpoint organization
- [x] Group endpoints by entity type

## Decisions

1. **Project Structure**: Using a layered architecture to separate concerns:
   - API Layer: Handles HTTP requests and routing
   - Data Layer: Manages database access and entity models
   - Request Processing Layer: Contains business logic and orchestration
   - Model Layer: Contains DTOs for data transfer between layers

2. **Minimal API Approach**: Using ASP.NET Core Minimal APIs instead of controllers for:
   - Reduced boilerplate code
   - Improved performance
   - More concise and readable code
   - Easier maintenance

3. **In-Memory Database**: Using EF Core's in-memory provider for:
   - Simplified development and testing
   - No external database dependencies
   - Quick setup and demonstration

4. **Request Processor Pattern**: Implementing a request processor pattern to:
   - Separate business logic from API endpoints
   - Enable unit testing of business logic
   - Provide a clear structure for handling requests

## Implementation Details

**Key Files and Their Responsibilities:**

### Data Layer (`MyProject.Data`)

**Entity Classes:**

```csharp
// Product.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyProject.Data
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}

// Order.cs
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MyProject.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
```

**Database Context:**

```csharp
// DemoDbContext.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MyProject.Data
{
    public class DemoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "DemoDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Orders);
        }

        public static void SeedData() // only required for dev and testing
        {
            using var context = new DemoDbContext();
            context.Database.EnsureCreated();

            var products = Enumerable.Range(1, 10).Select(i => new Product()
            {
                Name = $"Product {i}",
                ProductId = i
            });

            context.Products.AddRangeAsync(products);
            context.SaveChanges();

            var rnd = new Random();

            var orders = Enumerable.Range(1, 10).Select(i => new Order()
            {
                OrderId = i,
                Customer = $"Customer {i}",
                Products = context.Products.OrderBy(_ => rnd.Next()).Take(5).ToList()
            });

            context.Orders.AddRangeAsync(orders);
            context.SaveChanges();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
```

### Model Layer (`MyProject.Dtos`)

**DTOs:**

```csharp
// ProductDto.cs
namespace MyProject.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}

// OrderDto.cs
using System.Collections.Generic;

namespace MyProject.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string Customer { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
```

### Request Processing Layer (`MyProject.RequestProcessing`)

**Request/Response Objects:**

```csharp
// Features/GetOrder/GetOrderRequest.cs
namespace MyProject.RequestProcessing.Features.GetOrder
{
    public class GetOrderRequest
    {
        public int OrderId { get; set; }
    }
}

// Features/GetOrder/GetOrderResponse.cs
using MyProject.Dtos;

namespace MyProject.RequestProcessing.Features.GetOrder
{
    public class GetOrderResponse
    {
        public OrderDto Result { get; set; }
    }
}
```

**Request Processors:**

```csharp
// Features/GetOrder/GetOrderRequestProcessor.cs
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.RequestProcessing.Features.GetOrder
{
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
}

// Features/GetProduct/GetProductRequestProcessor.cs
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Dtos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyProject.RequestProcessing.Features.GetProduct
{
    public class GetProductRequestProcessor
    {
        private readonly DemoDbContext _demoDbContext;

        public GetProductRequestProcessor(DemoDbContext demoDbContext)
        {
            _demoDbContext = demoDbContext;
        }

        public async Task<GetProductResponse> HandleAsync(GetProductRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _demoDbContext.Products
                .Where(p => p.ProductId == request.ProductId)
                .Select(p => new ProductDto()
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return new GetProductResponse()
            {
                Result = result
            };
        }
    }
}
```

### API Layer (`MyProject.Api`)

**Program.cs:**

```csharp
using System.Reflection;
using MyProject.Data;
using MyProject.RequestProcessing.Features.GetOrder;
using MyProject.RequestProcessing.Features.GetProduct;
using MyProject.RequestProcessing.Features.CreateOrder;
using MyProject.RequestProcessing.Features.UpdateOrder;
using MyProject.RequestProcessing.Features.DeleteOrder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register request processors
builder.Services.AddTransient<GetOrderRequestProcessor>();
builder.Services.AddTransient<GetProductRequestProcessor>();
builder.Services.AddTransient<CreateOrderRequestProcessor>();
builder.Services.AddTransient<UpdateOrderRequestProcessor>();
builder.Services.AddTransient<DeleteOrderRequestProcessor>();

// Register database context
builder.Services.AddDbContext<DemoDbContext>();

// Seed data for development
DemoDbContext.SeedData();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map API endpoints
app.MapOrderEndpoints();
app.MapProductEndpoints();

app.Run();
```

**Endpoint Organization:**

```csharp
// OrderEndpoints.cs
using MyProject.Dtos;
using MyProject.RequestProcessing.Features.GetOrder;
using MyProject.RequestProcessing.Features.CreateOrder;
using MyProject.RequestProcessing.Features.UpdateOrder;
using MyProject.RequestProcessing.Features.DeleteOrder;

namespace MyProject.Api
{
    public static class OrderEndpointsExtensions
    {
        public static WebApplication MapOrderEndpoints(this WebApplication app)
        {
            // GET all orders
            app.MapGet("api/orders", async (GetAllOrdersRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new GetAllOrdersRequest());
                return Results.Ok(result.Results);
            })
            .WithName("GetAllOrders")
            .WithOpenApi();

            // GET order by ID
            app.MapGet("api/orders/{orderId}", async (int orderId, GetOrderRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new GetOrderRequest { OrderId = orderId });
                return result.Result != null ? Results.Ok(result.Result) : Results.NotFound();
            })
            .WithName("GetOrder")
            .WithOpenApi();

            // POST create new order
            app.MapPost("api/orders", async (OrderDto orderDto, CreateOrderRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new CreateOrderRequest { Order = orderDto });
                return Results.Created($"/api/orders/{result.OrderId}", result);
            })
            .WithName("CreateOrder")
            .WithOpenApi();

            // PUT update order
            app.MapPut("api/orders/{orderId}", async (int orderId, OrderDto orderDto, UpdateOrderRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new UpdateOrderRequest { OrderId = orderId, Order = orderDto });
                return result.WasUpdated ? Results.NoContent() : Results.NotFound();
            })
            .WithName("UpdateOrder")
            .WithOpenApi();

            // DELETE order
            app.MapDelete("api/orders/{orderId}", async (int orderId, DeleteOrderRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new DeleteOrderRequest { OrderId = orderId });
                return result.WasDeleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteOrder")
            .WithOpenApi();
            
            return app;
        }
    }
}

// ProductEndpoints.cs
using MyProject.Dtos;
using MyProject.RequestProcessing.Features.GetProduct;
using MyProject.RequestProcessing.Features.CreateProduct;
using MyProject.RequestProcessing.Features.UpdateProduct;
using MyProject.RequestProcessing.Features.DeleteProduct;

namespace MyProject.Api
{
    public static class ProductEndpointsExtensions
    {
        public static WebApplication MapProductEndpoints(this WebApplication app)
        {
            // GET all products
            app.MapGet("api/products", async (GetAllProductsRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new GetAllProductsRequest());
                return Results.Ok(result.Results);
            })
            .WithName("GetAllProducts")
            .WithOpenApi();

            // GET product by ID
            app.MapGet("api/products/{productId}", async (int productId, GetProductRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new GetProductRequest { ProductId = productId });
                return result.Result != null ? Results.Ok(result.Result) : Results.NotFound();
            })
            .WithName("GetProduct")
            .WithOpenApi();

            // POST create new product
            app.MapPost("api/products", async (ProductDto productDto, CreateProductRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new CreateProductRequest { Product = productDto });
                return Results.Created($"/api/products/{result.ProductId}", result);
            })
            .WithName("CreateProduct")
            .WithOpenApi();

            // PUT update product
            app.MapPut("api/products/{productId}", async (int productId, ProductDto productDto, UpdateProductRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new UpdateProductRequest { ProductId = productId, Product = productDto });
                return result.WasUpdated ? Results.NoContent() : Results.NotFound();
            })
            .WithName("UpdateProduct")
            .WithOpenApi();

            // DELETE product
            app.MapDelete("api/products/{productId}", async (int productId, DeleteProductRequestProcessor processor) =>
            {
                var result = await processor.HandleAsync(new DeleteProductRequest { ProductId = productId });
                return result.WasDeleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteProduct")
            .WithOpenApi();
            
            return app;
        }
    }
}
```

## Changes Made

1. Created a specification document for the ASP.NET Core Minimal API architecture at `.github/.copilot/specifications/application_architecture/aspnet-core-minimal-api.spec.md`
2. The specification documents:
   - The four-layer architecture (API, Data, Request Processing, and DTO layers)
   - Implementation guidelines for using ASP.NET Core Minimal APIs
   - Best practices for organizing endpoints using extension methods
   - The Request Processor pattern for handling business logic
   - Good and bad implementation examples

## Before/After Comparison

**Before:** No formal specification existed for the ASP.NET Core Minimal API architecture.

**After:** A comprehensive specification has been created that:
- Defines the architecture and implementation guidelines
- Documents best practices and coding standards
- Provides clear examples of good and bad implementations
- Serves as a reference for the development team

## References

- ASP.NET Core documentation: Used for understanding Minimal API best practices
- Entity Framework Core documentation: Referenced for database context setup
- Domain knowledge from provided project structure: Used as the foundation for the architecture
- Specification template: Used for structuring the specification document