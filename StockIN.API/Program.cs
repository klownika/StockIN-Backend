using Microsoft.EntityFrameworkCore;
using StockIN.API.Data;
using StockIN.API.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Connection string encontrada: {!string.IsNullOrEmpty(connectionString)}");

builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();


//Cuando algún Controller o Service necesite AppDbContext, se le da una instancia conectada a PostgreSQL mediante el paquete Npgsql

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
//guardar y ejecutar "dotnet build"

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();