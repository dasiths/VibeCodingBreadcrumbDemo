using CarRental.Dtos.Requests;
using CarRental.RequestProcessing.Makes;

namespace CarRental.Api.Endpoints;

public static class MakeEndpoints
{
    public static WebApplication MapMakeEndpoints(this WebApplication app)
    {
        app.MapGet("/api/makes/{makeId}", async (int makeId, 
                                                GetMakeRequestProcessor processor,
                                                CancellationToken cancellationToken) =>
        {
            var request = new GetMakeRequest { MakeId = makeId };
            var response = await processor.HandleAsync(request, cancellationToken);
            
            return response.Make != null 
                ? Results.Ok(response.Make) 
                : Results.NotFound();
        })
        .WithName("GetMake")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get a make by its ID";
            operation.Description = "Retrieves detailed information about a specific vehicle manufacturer";
            return operation;
        });
        
        app.MapGet("/api/makes", async (GetAllMakesRequestProcessor processor,
                                       CancellationToken cancellationToken) =>
        {
            var request = new GetAllMakesRequest();
            var response = await processor.HandleAsync(request, cancellationToken);
            return Results.Ok(response.Makes);
        })
        .WithName("GetAllMakes")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get all vehicle makes";
            operation.Description = "Retrieves a list of all vehicle manufacturers";
            return operation;
        });
        
        return app;
    }
}