using CarRental.Dtos.Requests;
using CarRental.RequestProcessing.Vehicles;

namespace CarRental.Api.Endpoints;

public static class VehicleEndpoints
{
    public static WebApplication MapVehicleEndpoints(this WebApplication app)
    {
        app.MapGet("/api/vehicles/{vehicleId}", async (int vehicleId, 
                                                      GetVehicleRequestProcessor processor,
                                                      CancellationToken cancellationToken) =>
        {
            var request = new GetVehicleRequest { VehicleId = vehicleId };
            var response = await processor.HandleAsync(request, cancellationToken);
            
            return response.Vehicle != null 
                ? Results.Ok(response.Vehicle) 
                : Results.NotFound();
        })
        .WithName("GetVehicle")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get a vehicle by its ID";
            operation.Description = "Retrieves detailed information about a specific vehicle";
            return operation;
        });
        
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
        .WithName("GetAllVehicles")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get a paginated list of vehicles";
            operation.Description = "Retrieves a paginated list of vehicles with optional search criteria";
            return operation;
        });
        
        return app;
    }
}