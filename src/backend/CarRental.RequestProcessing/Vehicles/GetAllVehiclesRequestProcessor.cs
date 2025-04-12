using CarRental.Dtos;
using CarRental.Dtos.Requests;
using CarRental.Dtos.Responses;

namespace CarRental.RequestProcessing.Vehicles;

public class GetAllVehiclesRequestProcessor
{
    // Since we're using hardcoded data for now, no database context is needed
    
    public Task<GetAllVehiclesResponse> HandleAsync(GetAllVehiclesRequest request, CancellationToken cancellationToken = default)
    {
        // Get sample data
        var allVehicles = GetSampleVehicles();
        
        // Apply search filter if provided
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            allVehicles = allVehicles
                .Where(v => v.MakeName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           v.ModelName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           v.Color.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           v.LicensePlate.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        // Count total results before pagination
        var totalCount = allVehicles.Count;
        
        // Apply pagination
        var pagedVehicles = allVehicles
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
        
        return Task.FromResult(new GetAllVehiclesResponse
        {
            Vehicles = pagedVehicles,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
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
            },
            new VehicleDto
            {
                VehicleId = 4,
                VIN = "WVWZZZ3CZJE123456",
                LicensePlate = "VW2023",
                Year = 2023,
                Color = "Gray",
                OdometerReading = 3000,
                Status = "Available",
                DailyRentalRate = 85.00m,
                PurchaseDate = new DateTime(2022, 11, 5),
                CurrentLocation = "Downtown Branch",
                ModelId = 4,
                ModelName = "Golf",
                MakeId = 4,
                MakeName = "Volkswagen",
                Tags = new List<TagDto>
                {
                    new TagDto { TagId = 6, Name = "Hatchback", Type = "Category" },
                    new TagDto { TagId = 7, Name = "GPS", Type = "Feature" }
                }
            },
            new VehicleDto
            {
                VehicleId = 5,
                VIN = "JTDZN3EU0EJ123456",
                LicensePlate = "HYB789",
                Year = 2022,
                Color = "Silver",
                OdometerReading = 18000,
                Status = "Available",
                DailyRentalRate = 70.00m,
                PurchaseDate = new DateTime(2021, 8, 12),
                CurrentLocation = "Main Branch",
                ModelId = 5,
                ModelName = "Prius",
                MakeId = 5,
                MakeName = "Toyota",
                Tags = new List<TagDto>
                {
                    new TagDto { TagId = 8, Name = "Hybrid", Type = "Category" },
                    new TagDto { TagId = 9, Name = "Eco-friendly", Type = "Feature" }
                }
            }
        };
    }
}