using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;

namespace RestaurantReservation.API.Endpoints;

// Container class for reservation-related endpoints
public static class ReservationEndpoints
{
    // Extension method to map reservation endpoints to the WebApp
    public static void MapReservationEndpoints(this WebApplication app)
    {
        #region CRUD Reservation Endpoints
        // Read - Get all reservations
        app.MapGet("/api/reservations", async (ReservationRepository repo) =>
        {
            var reservations = await repo.GetAll();
            return Results.Ok(reservations);
        }).RequireAuthorization();

        // Read - Get reservation by ID
        app.MapGet("/api/reservations/{id}", async (ReservationRepository repo, int id) =>
        {
            var reservationById = await repo.GetById(id);

            if (reservationById == null) 
            {
                return Results.NotFound();
            }
            return Results.Ok(reservationById);
        }).RequireAuthorization();

        // Create
        app.MapPost("/api/reservations", async (ReservationRepository repo, Reservation reservation) =>
        {
            var created = await repo.Create(reservation);
            return Results.Created($"/api/reservations/{created.ReservationId}", created);
        }).RequireAuthorization();

        // Update
        app.MapPut("/api/reservations/{id}", async (ReservationRepository repo, int id, Reservation reservation) =>
        {
            if (id != reservation.ReservationId)
            {
                return Results.BadRequest("ID mismatch");
            }

            var success = await repo.Update(reservation);

            if (!success)
            {
                return Results.NotFound();
            }
            return Results.Ok(reservation);
        }).RequireAuthorization();

        // Delete
        app.MapDelete("/api/reservations/{id}", async (ReservationRepository repo, int id) =>
        {
            var success = await repo.Delete(id);

            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        }).RequireAuthorization();
        #endregion CRUD Reservation Endpoints

        // Additional endpoints
        app.MapGet("/api/employees/managers", async (EmployeeRepository repo) =>
        {
            var managers = await repo.ListManagers();
            return Results.Ok(managers);
        }).RequireAuthorization();

        app.MapGet("/api/reservations/customer/{customerId}", async (ReservationRepository repo, int customerId) =>
        {
            var reservationByCustomer = await repo.GetReservationsByCustomer(customerId);
            return Results.Ok(reservationByCustomer);
        }).RequireAuthorization();

        app.MapGet("/api/reservations/{reservationId}/orders", async (OrderRepository orderRepo, int reservationId) =>
        {
            var listOrders = await orderRepo.ListOrdersAndMenuItems(reservationId);
            return Results.Ok(listOrders);
        }).RequireAuthorization();

        app.MapGet("/api/reservations/{reservationId}/menu-items", async (MenuItemRepository menuRepo, int reservationId) =>
        {
            var listMenuItems = await menuRepo.ListOrderedMenuItems(reservationId);
            return Results.Ok(listMenuItems);
        }).RequireAuthorization();

        app.MapGet("/api/employees/{employeeId}/average-order-amount", async (EmployeeRepository repo, int employeeId) =>
        {
            var averageOrderAmount = await repo.GetAverageOrderAmount(employeeId);
            return Results.Ok(averageOrderAmount);
        }).RequireAuthorization();
    }
}