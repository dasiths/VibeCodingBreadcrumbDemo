using CarRental.Dtos;
using CarRental.Dtos.Requests;
using CarRental.Dtos.Responses;

namespace CarRental.RequestProcessing.Vehicles;

public class GetVehicleRequestProcessor
{
    // Since we're using hardcoded data for now, no database context is needed
    
    public Task<GetVehicleResponse> HandleAsync(GetVehicleRequest request, CancellationToken cancellationToken = default)
    {
        // Hardcoded sample data for initial development
        var vehicle = GetSampleVehicles().FirstOrDefault(v => v.VehicleId == request.VehicleId);
        
        return Task.FromResult(new GetVehicleResponse
        {
            Vehicle = vehicle
        });
    }
    
    // Sample data helper - would be replaced with actual data access logic later
    private static List<VehicleDto> GetSampleVehicles()
    {
        return new List<VehicleDto>
        {
            new VehicleDto
            {
                VehicleId = 1,
                VIN = "1HGCM82633A123456",
                LicensePlate = "ABC123",
                Year = 2022,
                Color = "Blue",
                OdometerReading = 15000,
                Status = "Available",
                DailyRentalRate = 65.00m,
                PurchaseDate = new DateTime(2021, 10, 15),
                CurrentLocation = "Main Branch",
                ModelId = 1,
                ModelName = "Accord",
                MakeId = 1,
                MakeName = "Honda",
                Tags = new List<TagDto>
                {
                    new TagDto { TagId = 1, Name = "Sedan", Type = "Category" },
                    new TagDto { TagId = 4, Name = "Bluetooth", Type = "Feature" }
                }
            },
            new VehicleDto
            {
                VehicleId = 2,
                VIN = "JM1BL1V53C1234567",
                LicensePlate = "XYZ789",
                Year = 2023,
                Color = "Red",
                OdometerReading = 5000,
                Status = "Available",
                DailyRentalRate = 75.00m,
                PurchaseDate = new DateTime(2022, 3, 20),
                CurrentLocation = "Airport Branch",
                ModelId = 2,
                ModelName = "Mazda3",
                MakeId = 2,
                MakeName = "Mazda",
                Tags = new List<TagDto>
                {
                    new TagDto { TagId = 1, Name = "Sedan", Type = "Category" },
                    new TagDto { TagId = 2, Name = "Sunroof", Type = "Feature" }
                }
            },
            new VehicleDto
            {
                VehicleId = 3,
                VIN = "5YJSA1E40GF123456",
                LicensePlate = "EV2022",
                Year = 2022,
                Color = "White",
                OdometerReading = 12000,
                Status = "Maintenance",
                DailyRentalRate = 120.00m,
                PurchaseDate = new DateTime(2021, 5, 10),
                CurrentLocation = "Service Center",
                ModelId = 3,
                ModelName = "Model S",
                MakeId = 3,
                MakeName = "Tesla",
                Tags = new List<TagDto>
                {
                    new TagDto { TagId = 3, Name = "Electric", Type = "Category" },
                    new TagDto { TagId = 5, Name = "Autopilot", Type = "Feature" }
                }
            }
        };
    }
}