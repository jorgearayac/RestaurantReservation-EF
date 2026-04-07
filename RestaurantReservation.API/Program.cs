using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
var connectionString = builder.Configuration.GetConnectionString("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = RestaurantReservationCore")
    ?? throw new InvalidOperationException("Connection string not found."); 
    
builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString))); 

builder.Services.AddScoped<ReservationRepository>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// CRUD endpoints for Reservations
//app.MapGet("/api/reservations", async (ReservationRepository repository) =>
//{
//    var reservations = await repository.GetAllReservationsAsync();
//    return Results.Ok(reservations);
//});