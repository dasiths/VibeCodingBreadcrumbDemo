namespace CarRental.Dtos;

public class VehicleDto
{
    public int VehicleId { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public int OdometerReading { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal DailyRentalRate { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string? CurrentLocation { get; set; }
    public int ModelId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int MakeId { get; set; }
    public string MakeName { get; set; } = string.Empty;
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
}