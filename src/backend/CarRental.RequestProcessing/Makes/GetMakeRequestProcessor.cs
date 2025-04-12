using CarRental.Dtos;
using CarRental.Dtos.Requests;
using CarRental.Dtos.Responses;

namespace CarRental.RequestProcessing.Makes;

public class GetMakeRequestProcessor
{
    public Task<GetMakeResponse> HandleAsync(GetMakeRequest request, CancellationToken cancellationToken = default)
    {
        // Hardcoded sample data for initial development
        var make = GetSampleMakes().FirstOrDefault(m => m.MakeId == request.MakeId);
        
        return Task.FromResult(new GetMakeResponse
        {
            Make = make
        });
    }
    
    // Sample data helper - would be replaced with actual data access logic later
    private static List<MakeDto> GetSampleMakes()
    {
        return new List<MakeDto>
        {
            new MakeDto
            {
                MakeId = 1,
                Name = "Honda",
                CountryOfOrigin = "Japan",
                FoundedYear = 1948,
                LogoUrl = "https://example.com/logos/honda.png"
            },
            new MakeDto
            {
                MakeId = 2,
                Name = "Mazda",
                CountryOfOrigin = "Japan",
                FoundedYear = 1920,
                LogoUrl = "https://example.com/logos/mazda.png"
            },
            new MakeDto
            {
                MakeId = 3,
                Name = "Tesla",
                CountryOfOrigin = "United States",
                FoundedYear = 2003,
                LogoUrl = "https://example.com/logos/tesla.png"
            },
            new MakeDto
            {
                MakeId = 4,
                Name = "Volkswagen",
                CountryOfOrigin = "Germany",
                FoundedYear = 1937,
                LogoUrl = "https://example.com/logos/volkswagen.png"
            },
            new MakeDto
            {
                MakeId = 5,
                Name = "Toyota",
                CountryOfOrigin = "Japan",
                FoundedYear = 1937,
                LogoUrl = "https://example.com/logos/toyota.png"
            }
        };
    }
}