using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestaurantReservation.API.Endpoints;

// Container class for Authentication Endpoints
public static class AuthenticationEndpoint
{
    public static void MapAuthenticationEndpoint(this WebApplication app, IConfiguration configuration)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request) =>
        {
            if (request.Username != "admin" || request.Password != "password")
            {
                return Results.Unauthorized();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
                );

            return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token)});
        }).AllowAnonymous();
    }
    record LoginRequest(string Username, string Password);
}
