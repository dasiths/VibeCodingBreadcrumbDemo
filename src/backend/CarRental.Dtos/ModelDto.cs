namespace CarRental.Dtos;

public class ModelDto
{
    public int ModelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int YearIntroduced { get; set; }
    public string? Class { get; set; }
    public int DefaultSeatingCapacity { get; set; }
    public double BaseFuelEfficiency { get; set; }
    public bool IsActive { get; set; }
    public int MakeId { get; set; }
    public string MakeName { get; set; } = string.Empty;
}