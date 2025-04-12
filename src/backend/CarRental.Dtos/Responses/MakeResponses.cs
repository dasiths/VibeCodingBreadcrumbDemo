namespace CarRental.Dtos.Responses;

public class GetMakeResponse
{
    public MakeDto? Make { get; set; }
}

public class GetAllMakesResponse
{
    public List<MakeDto> Makes { get; set; } = new List<MakeDto>();
}