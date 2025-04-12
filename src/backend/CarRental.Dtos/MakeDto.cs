namespace CarRental.Dtos;

public class MakeDto
{
    public int MakeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CountryOfOrigin { get; set; }
    public int? FoundedYear { get; set; }
    public string? LogoUrl { get; set; }
}