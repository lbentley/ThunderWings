using ThunderWings.Interfaces;
using ThunderWings.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IAircraftService>(provider =>
{
    string filePath = Path.Combine(AppContext.BaseDirectory, "data", "aircraft.json");
    return new AircraftService(filePath);
});

builder.Services.AddSingleton<IBasketService, BasketService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
