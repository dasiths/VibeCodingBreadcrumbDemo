namespace CarRental.Dtos.Responses;

public class GetModelResponse
{
    public ModelDto? Model { get; set; }
}

public class GetModelsByMakeResponse
{
    public List<ModelDto> Models { get; set; } = new List<ModelDto>();
    public int MakeId { get; set; }
    public string MakeName { get; set; } = string.Empty;
}