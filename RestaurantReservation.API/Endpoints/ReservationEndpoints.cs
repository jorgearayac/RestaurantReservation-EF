using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories;
using RestaurantReservation.API.Validators;

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
        })
            .RequireAuthorization()
            .WithName("GetAllReservations")
            .WithSummary("Get all Reservations")
            .WithDescription("Returns a list of all the reservations in the system")
            .Produces<List<Reservation>>(200)
            .Produces(401);

        // Read - Get reservation by ID
        app.MapGet("/api/reservations/{id}", async (ReservationRepository repo, int id) =>
        {
            var reservationById = await repo.GetById(id);

            if (reservationById == null) 
            {
                return Results.NotFound();
            }
            return Results.Ok(reservationById);
        })
            .RequireAuthorization()
            .WithName("GetReservationById")
            .WithSummary("Get a reservation by ID")
            .WithDescription("Returns a reservation by its ID")
            .Produces<Reservation>(200)
            .Produces(404)
            .Produces(401);

        // Create
        app.MapPost("/api/reservations", async (ReservationRepository repo, Reservation reservation, ReservationValidator validator) =>
        {
            var validationResult = await validator.ValidateAsync(reservation);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var created = await repo.Create(reservation);
            return Results.Created($"/api/reservations/{created.ReservationId}", created);
        })
            .RequireAuthorization()
            .WithName("CreateReservation")
            .WithSummary("Create a reservation")
            .WithDescription("Creates a new reservation in the system")
            .Produces<Reservation>(201)
            .Produces(400)
            .Produces(401);

        // Update
        app.MapPut("/api/reservations/{id}", async (ReservationRepository repo, int id, Reservation reservation, ReservationValidator validator) =>
        {
            if (id != reservation.ReservationId)
            {
                return Results.BadRequest("ID mismatch");
            }

            var validationResult = await validator.ValidateAsync(reservation);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }

            var success = await repo.Update(reservation);

            if (!success)
            {
                return Results.NotFound();
            }
            return Results.Ok(reservation);
        })
            .RequireAuthorization()
            .WithName("UpdateReservation")
            .WithSummary("Update a reservation")
            .WithDescription("Updates an existing reservation in the system")
            .Produces<Reservation>(200)
            .Produces(400)
            .Produces(404)
            .Produces(401);

        // Delete
        app.MapDelete("/api/reservations/{id}", async (ReservationRepository repo, int id) =>
        {
            var success = await repo.Delete(id);

            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        })
            .RequireAuthorization()
            .WithName("DeleteReservation")
            .WithSummary("Delete a reservation")
            .WithDescription("Deletes an existing reservation in the system")
            .Produces(204)
            .Produces(404)
            .Produces(401);
        #endregion CRUD Reservation Endpoints

        // Additional endpoints
        #region Additional Endpoints
        app.MapGet("/api/employees/managers", async (EmployeeRepository repo) =>
        {
            var managers = await repo.ListManagers();
            return Results.Ok(managers);
        })
            .RequireAuthorization()
            .WithName("GetManagers")
            .WithSummary("Get all managers")
            .WithDescription("Returns a list of all managers")
            .Produces<List<Employee>>(200)
            .Produces(401);

        app.MapGet("/api/reservations/customer/{customerId}", async (ReservationRepository repo, int customerId) =>
        {
            var reservationByCustomer = await repo.GetReservationsByCustomer(customerId);
            return Results.Ok(reservationByCustomer);
        })
            .RequireAuthorization()
            .WithName("GetReservationsByCustomer")
            .WithSummary("Get reservations by customer ID")
            .WithDescription("Returns a list of reservations for a specific customer")
            .Produces<List<Reservation>>(200)
            .Produces(401);

        app.MapGet("/api/reservations/{reservationId}/orders", async (OrderRepository orderRepo, int reservationId) =>
        {
            var listOrders = await orderRepo.ListOrdersAndMenuItems(reservationId);
            return Results.Ok(listOrders);
        })
            .RequireAuthorization()
            .WithName("GetOrdersByReservation")
            .WithSummary("Get orders by reservation ID")
            .WithDescription("Returns a list of orders for a specific reservation")
            .Produces<List<Order>>(200)
            .Produces(401);

        app.MapGet("/api/reservations/{reservationId}/menu-items", async (MenuItemRepository menuRepo, int reservationId) =>
        {
            var listMenuItems = await menuRepo.ListOrderedMenuItems(reservationId);
            return Results.Ok(listMenuItems);
        })
            .RequireAuthorization()
            .WithName("GetMenuItemsByReservation")
            .WithSummary("Get menu items by reservation ID")
            .WithDescription("Returns a list of menu items for a specific reservation")
            .Produces<List<MenuItem>>(200)
            .Produces(401);

        app.MapGet("/api/employees/{employeeId}/average-order-amount", async (EmployeeRepository repo, int employeeId) =>
        {
            var averageOrderAmount = await repo.GetAverageOrderAmount(employeeId);
            return Results.Ok(averageOrderAmount);
        })
            .RequireAuthorization()
            .WithName("GetAverageOrderAmount")
            .WithSummary("Get average order amount by employee ID")
            .WithDescription("Returns the average order amount for a specific employee. To do: Handle case when employee does not exist")
            .Produces<decimal>(200)
            .Produces(401);
        #endregion Additional Endpoints
    }
}