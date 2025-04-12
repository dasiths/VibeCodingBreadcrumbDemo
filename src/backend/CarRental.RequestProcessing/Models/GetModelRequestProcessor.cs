using CarRental.Dtos;
using CarRental.Dtos.Requests;
using CarRental.Dtos.Responses;

namespace CarRental.RequestProcessing.Models;

public class GetModelRequestProcessor
{
    public Task<GetModelResponse> HandleAsync(GetModelRequest request, CancellationToken cancellationToken = default)
    {
        // Hardcoded sample data for initial development
        var model = GetSampleModels().FirstOrDefault(m => m.ModelId == request.ModelId);
        
        return Task.FromResult(new GetModelResponse
        {
            Model = model
        });
    }
    
    // Sample data helper - would be replaced with actual data access logic later
    private static List<ModelDto> GetSampleModels()
    {
        return new List<ModelDto>
        {
            new ModelDto
            {
                ModelId = 1,
                Name = "Accord",
                YearIntroduced = 1976,
                Class = "Sedan",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 30.5,
                IsActive = true,
                MakeId = 1,
                MakeName = "Honda"
            },
            new ModelDto
            {
                ModelId = 2,
                Name = "Mazda3",
                YearIntroduced = 2003,
                Class = "Sedan",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 28.0,
                IsActive = true,
                MakeId = 2,
                MakeName = "Mazda"
            },
            new ModelDto
            {
                ModelId = 3,
                Name = "Model S",
                YearIntroduced = 2012,
                Class = "Luxury Sedan",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 100.0, // Electric - MPGe
                IsActive = true,
                MakeId = 3,
                MakeName = "Tesla"
            },
            new ModelDto
            {
                ModelId = 4,
                Name = "Golf",
                YearIntroduced = 1974,
                Class = "Hatchback",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 32.0,
                IsActive = true,
                MakeId = 4,
                MakeName = "Volkswagen"
            },
            new ModelDto
            {
                ModelId = 5,
                Name = "Prius",
                YearIntroduced = 1997,
                Class = "Hybrid",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 54.0,
                IsActive = true,
                MakeId = 5,
                MakeName = "Toyota"
            },
            new ModelDto
            {
                ModelId = 6,
                Name = "Civic",
                YearIntroduced = 1972,
                Class = "Compact",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 32.5,
                IsActive = true,
                MakeId = 1,
                MakeName = "Honda"
            },
            new ModelDto
            {
                ModelId = 7,
                Name = "CR-V",
                YearIntroduced = 1995,
                Class = "SUV",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 28.0,
                IsActive = true,
                MakeId = 1,
                MakeName = "Honda"
            },
            new ModelDto
            {
                ModelId = 8,
                Name = "Camry",
                YearIntroduced = 1982,
                Class = "Sedan",
                DefaultSeatingCapacity = 5,
                BaseFuelEfficiency = 32.0,
                IsActive = true,
                MakeId = 5,
                MakeName = "Toyota"
            }
        };
    }
}