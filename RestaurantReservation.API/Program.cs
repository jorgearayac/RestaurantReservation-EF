using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Endpoints;
using RestaurantReservation.API.Validators;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found."); 
    
builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register repositories for dependency injection
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<ReservationValidator>();


// Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware to redirect HTTP requests to HTTPS
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Authentication Endpoint
app.MapAuthenticationEndpoint(builder.Configuration);

// CRUD endpoints for Reservations
app.MapReservationEndpoints();

app.Run();