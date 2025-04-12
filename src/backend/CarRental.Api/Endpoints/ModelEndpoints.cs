using CarRental.Dtos.Requests;
using CarRental.RequestProcessing.Models;

namespace CarRental.Api.Endpoints;

public static class ModelEndpoints
{
    public static WebApplication MapModelEndpoints(this WebApplication app)
    {
        app.MapGet("/api/models/{modelId}", async (int modelId, 
                                                 GetModelRequestProcessor processor,
                                                 CancellationToken cancellationToken) =>
        {
            var request = new GetModelRequest { ModelId = modelId };
            var response = await processor.HandleAsync(request, cancellationToken);
            
            return response.Model != null 
                ? Results.Ok(response.Model) 
                : Results.NotFound();
        })
        .WithName("GetModel")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get a model by its ID";
            operation.Description = "Retrieves detailed information about a specific vehicle model";
            return operation;
        });
        
        app.MapGet("/api/makes/{makeId}/models", async (int makeId, 
                                                      GetModelsByMakeRequestProcessor processor,
                                                      CancellationToken cancellationToken) =>
        {
            var request = new GetModelsByMakeRequest { MakeId = makeId };
            var response = await processor.HandleAsync(request, cancellationToken);
            
            return response.Models.Any()
                ? Results.Ok(response)
                : Results.NotFound();
        })
        .WithName("GetModelsByMake")
        .WithOpenApi(operation => 
        {
            operation.Summary = "Get all models for a specific make";
            operation.Description = "Retrieves a list of all vehicle models for a given manufacturer";
            return operation;
        });
        
        return app;
    }
}