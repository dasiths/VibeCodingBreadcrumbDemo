namespace CarRental.Dtos.Responses;

public class GetVehicleResponse
{
    public VehicleDto? Vehicle { get; set; }
}

public class GetAllVehiclesResponse
{
    public List<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}