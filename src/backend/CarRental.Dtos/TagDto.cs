namespace CarRental.Dtos;

public class TagDto
{
    public int TagId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
}