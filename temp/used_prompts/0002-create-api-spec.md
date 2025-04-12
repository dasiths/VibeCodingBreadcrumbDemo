I'm planning to use aspnet core for this project. Help me write a specification for the API based on the following.

## Project Structure

### 1. API Layer
**Project Name**: `MyProject.Api`

- **Purpose**: This project handles the HTTP requests and routes them to the appropriate request processors using ASP.NET Minimal APIs.
- **Dependencies**: 
  - `MyProject.Data` for database context.
  - `MyProject.RequestProcessing` for handling requests.

**Key File**: `Program.cs`
```csharp
using System.Reflection;
using MyProject.Data;
using MyProject.RequestProcessing.Features.GetOrder;
using MyProject.RequestProcessing.Features.GetProduct;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GetOrderRequestProcessor>();
builder.Services.AddTransient<GetProductRequestProcessor>();
builder.Services.AddDbContext<DemoDbContext>();

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

// Minimal API endpoint definitions
app.MapGet("api/orders/{orderId}", async (int orderId, GetOrderRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new GetOrderRequest { OrderId = orderId });
    return Results.Ok(result);
})
.WithName("GetOrder")
.WithOpenApi();

app.MapGet("api/products/{productId}", async (int productId, GetProductRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new GetProductRequest { ProductId = productId });
    return Results.Ok(result);
})
.WithName("GetProduct")
.WithOpenApi();

app.Run();
```

**Key Concept**: Instead of controllers, the API layer now uses Minimal API endpoints defined directly in `Program.cs`.

### 2. Data Layer
**Project Name**: `MyProject.Data`

- **Purpose**: This project manages the database context, domain entity models and data access logic.
- **Dependencies**: 
  - `Microsoft.EntityFrameworkCore` for database operations.

**Key File**: `DemoDbContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;

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

File `Order.cs`

```csharp
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public string Customer { get; set; }
    public List<Product> Products { get; set; }
}
```

### 3. Request Processing Layer
**Project Name**: `MyProject.RequestProcessing`

- **Purpose**: This project implements the use cases and application services. It contains all business logic of the application, acting as the orchestrator between the API and Data layers.
- **Dependencies**: 
  - `MyProject.Data` for accessing the database context.
  - `MyProject.Dtos` for dto models.

**Key File**: `GetOrderRequestProcessor.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Dtos;

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
            var result = await _demoDbContext.Orders.Include(o => o.Products).Where(o => o.OrderId == request.OrderId).Select(o =>
                new OrderDto()
                {
                    Customer = o.Customer,
                    OrderId = o.OrderId,
                    Products = o.Products.Select(p => new ProductDto()
                    {
                        ProductId = p.ProductId,
                        Name = p.Name
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            return new GetOrderResponse()
            {
                Result = result
            };
        }
    }
}
```

### 4. Model Layer
**Project Name**: `MyProject.Dtos`

- **Purpose**: This project contains only DTO (Data Transfer Object) type objects with minimal logic. It primarily consists of data structures used for communication between layers. These are not domain models that contain business logic.
- **Dependencies**: None.

**Key File**: `OrderDto.cs`
```csharp
public class OrderDto
{
    public int OrderId { get; set; }
    public string Customer { get; set; }
    public List<ProductDto> Products { get; set; }
}
```

## Minimal API Best Practices

### Organizing Endpoints

For larger applications, organize endpoints into extension methods:

```csharp
// Program.cs
var app = builder.Build();
// Configure app...

app.MapOrderEndpoints();
app.MapProductEndpoints();

app.Run();

// OrderEndpoints.cs
public static class OrderEndpointsExtensions
{
    public static WebApplication MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("api/orders", async (GetAllOrdersRequestProcessor processor) =>
        {
            var result = await processor.HandleAsync(new GetAllOrdersRequest());
            return Results.Ok(result);
        })
        .WithName("GetAllOrders")
        .WithOpenApi();

        app.MapGet("api/orders/{orderId}", async (int orderId, GetOrderRequestProcessor processor) =>
        {
            var result = await processor.HandleAsync(new GetOrderRequest { OrderId = orderId });
            return Results.Ok(result);
        })
        .WithName("GetOrder")
        .WithOpenApi();

        // More order endpoints...
        
        return app;
    }
}
```

### CRUD Operations with Minimal API

Here's how CRUD operations look with Minimal APIs:

```csharp
// GET all
app.MapGet("api/orders", async (GetAllOrdersRequestProcessor processor) => 
    Results.Ok(await processor.HandleAsync(new GetAllOrdersRequest())));

// GET by ID
app.MapGet("api/orders/{id}", async (int id, GetOrderRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new GetOrderRequest { OrderId = id });
    return result.Result != null ? Results.Ok(result.Result) : Results.NotFound();
});

// POST
app.MapPost("api/orders", async (OrderDto orderDto, CreateOrderRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new CreateOrderRequest { Order = orderDto });
    return Results.Created($"/api/orders/{result.OrderId}", result);
});

// PUT
app.MapPut("api/orders/{id}", async (int id, OrderDto orderDto, UpdateOrderRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new UpdateOrderRequest { OrderId = id, Order = orderDto });
    return result.WasUpdated ? Results.NoContent() : Results.NotFound();
});

// DELETE
app.MapDelete("api/orders/{id}", async (int id, DeleteOrderRequestProcessor processor) =>
{
    var result = await processor.HandleAsync(new DeleteOrderRequest { OrderId = id });
    return result.WasDeleted ? Results.NoContent() : Results.NotFound();
});
```

## Summary of Dependencies
- `MyProject.Api` depends on `MyProject.Data` and `MyProject.RequestProcessing`.
- `MyProject.RequestProcessing` depends on `MyProject.Data` and `MyProject.Dtos`.

This layout ensures that each project has a clear responsibility and the dependencies are well-defined, promoting a clean and maintainable architecture while leveraging the simplicity and performance benefits of ASP.NET Minimal APIs.