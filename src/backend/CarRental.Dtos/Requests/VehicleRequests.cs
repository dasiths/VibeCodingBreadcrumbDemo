namespace CarRental.Dtos.Requests;

public class GetVehicleRequest
{
    public int VehicleId { get; set; }
}

public class GetAllVehiclesRequest
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}