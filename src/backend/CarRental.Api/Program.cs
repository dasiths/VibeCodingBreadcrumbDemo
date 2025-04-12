using CarRental.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register request processors
builder.Services.AddScoped<CarRental.RequestProcessing.Vehicles.GetVehicleRequestProcessor>();
builder.Services.AddScoped<CarRental.RequestProcessing.Vehicles.GetAllVehiclesRequestProcessor>();
builder.Services.AddScoped<CarRental.RequestProcessing.Makes.GetMakeRequestProcessor>();
builder.Services.AddScoped<CarRental.RequestProcessing.Makes.GetAllMakesRequestProcessor>();
builder.Services.AddScoped<CarRental.RequestProcessing.Models.GetModelRequestProcessor>();
builder.Services.AddScoped<CarRental.RequestProcessing.Models.GetModelsByMakeRequestProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapVehicleEndpoints();
app.MapMakeEndpoints();
app.MapModelEndpoints();

app.Run();
