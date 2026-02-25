using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Services;
using System.ComponentModel.DataAnnotations;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Demo for Create/Update/Delete operations in Restaurant Reservation System");
        Console.WriteLine("=================================================");
        using var context = new RestaurantReservationDbContext();
        
        // Initialize services
        var customerService = new CustomerService(context);
        var employeeService = new EmployeeService(context);
        var menuItemService = new MenuItemService(context);
        var orderItemService = new OrderItemService(context);
        var orderService = new OrderService(context);
        var reservationService = new ReservationService(context);
        var restaurantService = new RestaurantService(context);
        var tableService = new TableService(context);

        var newCustomer = new Customer { FirstName = "Async", LastName = "Await", Email = "async@test.cl", PhoneNumber = "555555555" };
        var newEmployee = new Employee { RestaurantId = 1, FirstName = "John", LastName = "Doe", Position = "Waiter" };
        var newMenuItem = new MenuItem { RestaurantId = 1, Name = "Pasta", Description = "Delicious pasta with tomato sauce", Price = 12.99m };
        var newOrderItem = new OrderItem { OrderId = 1, ItemId = 1, Quantity = 2 };
        var newOrder = new Order { ReservationId = 1, EmployeeId = 1, OrderDate = DateTime.Now, TotalAmount = 25.98m };
        var newReservation = new Reservation { CustomerId = 1, RestaurantId = 1, TableId = 1, ReservationDate = DateTime.Now.AddDays(1), PartySize = 4 };
        var newRestaurant = new Restaurant { Name = "Testaurant", Address = "123 Test St", PhoneNumber = "555-1234", OpeningHours = "10am - 11pm" };
        var newTable = new Table { RestaurantId = 1, Capacity = 4 };

        // Create operations
        Console.WriteLine("Create operations:");
        Console.WriteLine("------------------");

        await customerService.Create(newCustomer);
        Console.WriteLine($"Customer created: {newCustomer.FirstName} {newCustomer.LastName} (ID: {newCustomer.CustomerId})");

        await employeeService.Create(newEmployee);
        Console.WriteLine($"Employee created: {newEmployee.FirstName} {newEmployee.LastName} (ID: {newEmployee.EmployeeId})");

        await menuItemService.Create(newMenuItem);
        Console.WriteLine($"Menu item created: {newMenuItem.Name} (ID: {newMenuItem.ItemId})");

        await orderItemService.Create(newOrderItem);
        Console.WriteLine($"Order item created: Order ID {newOrderItem.OrderId}, Item ID {newOrderItem.ItemId}, Quantity {newOrderItem.Quantity}");

        await orderService.Create(newOrder);
        Console.WriteLine($"Order created: ID {newOrder.OrderId}, Total Amount: {newOrder.TotalAmount}");

        await reservationService.Create(newReservation);
        Console.WriteLine($"Reservation created: ID {newReservation.ReservationId}, Party Size: {newReservation.PartySize}");

        await restaurantService.Create(newRestaurant);
        Console.WriteLine($"Restaurant created: {newRestaurant.Name} (ID: {newRestaurant.RestaurantId})");

        await tableService.Create(newTable);
        Console.WriteLine($"Table created: ID {newTable.TableId}, Capacity: {newTable.Capacity}");

        // Update operations
        Console.WriteLine("Update operations:");
        Console.WriteLine("------------------");
        await customerService.Update(new Customer { CustomerId = newCustomer.CustomerId, FirstName = "Updated", LastName = newCustomer.LastName, Email = newCustomer.Email, PhoneNumber = newCustomer.PhoneNumber });
        Console.WriteLine($"Customer updated: ID {newCustomer.CustomerId}. Change: Name to {newCustomer.FirstName}");

        await employeeService.Update(new Employee { EmployeeId = newEmployee.EmployeeId, RestaurantId = newEmployee.RestaurantId, FirstName = newEmployee.FirstName, LastName = newEmployee.LastName, Position = "Manager" });
        Console.WriteLine($"Employee updated: ID {newEmployee.EmployeeId}. Change: Position to {newEmployee.Position}");

        await menuItemService.Update(new MenuItem { ItemId = newMenuItem.ItemId, RestaurantId = newMenuItem.RestaurantId, Name = newMenuItem.Name, Description = newMenuItem.Description, Price = 14.99m });
        Console.WriteLine($"Menu item updated: ID {newMenuItem.ItemId}. Change: Price to {newMenuItem.Price}");

        await orderItemService.Update(new OrderItem { OrderItemId = newOrderItem.OrderItemId, OrderId = newOrderItem.OrderId, ItemId = newOrderItem.ItemId, Quantity = 3 });
        Console.WriteLine($"Order item updated: ID {newOrderItem.OrderItemId}. Change: Quantity to {newOrderItem.Quantity}");

        await orderService.Update(new Order { OrderId = newOrder.OrderId, ReservationId = newOrder.ReservationId, EmployeeId = newOrder.EmployeeId, OrderDate = newOrder.OrderDate, TotalAmount = 38.97m });
        Console.WriteLine($"Order updated: ID {newOrder.OrderId}. Change: Total Amount to {newOrder.TotalAmount}");

        await reservationService.Update(new Reservation { ReservationId = newReservation.ReservationId, CustomerId = newReservation.CustomerId, RestaurantId = newReservation.RestaurantId, TableId = newReservation.TableId, ReservationDate = newReservation.ReservationDate, PartySize = 5 });
        Console.WriteLine($"Reservation updated: ID {newReservation.ReservationId}. Change: Party Size to {newReservation.PartySize}");

        await restaurantService.Update(new Restaurant { RestaurantId = newRestaurant.RestaurantId, Name = newRestaurant.Name, Address = newRestaurant.Address, PhoneNumber = newRestaurant.PhoneNumber, OpeningHours = "9am - 10pm" });
        Console.WriteLine($"Restaurant updated: ID {newRestaurant.RestaurantId}. Change: Opening Hours to {newRestaurant.OpeningHours}");

        await tableService.Update(new Table { TableId = newTable.TableId, RestaurantId = newTable.RestaurantId, Capacity = 6 });
        Console.WriteLine($"Table updated: ID {newTable.TableId}. Change: Capacity to {newTable.Capacity}");

        // Delete operations
        Console.WriteLine("Delete operations:");
        Console.WriteLine("------------------");
        await customerService.Delete(newCustomer.CustomerId);
        Console.WriteLine($"Customer deleted: ID {newCustomer.CustomerId}");

        await employeeService.Delete(newEmployee.EmployeeId);
        Console.WriteLine($"Employee deleted: ID {newEmployee.EmployeeId}");

        await menuItemService.Delete(newMenuItem.ItemId);
        Console.WriteLine($"Menu item deleted: ID {newMenuItem.ItemId}");

        await orderItemService.Delete(newOrderItem.OrderItemId);
        Console.WriteLine($"Order item deleted: ID {newOrderItem.OrderItemId}");

        await orderService.Delete(newOrder.OrderId);
        Console.WriteLine($"Order deleted: ID {newOrder.OrderId}");

        await reservationService.Delete(newReservation.ReservationId);
        Console.WriteLine($"Reservation deleted: ID {newReservation.ReservationId}");

        await restaurantService.Delete(newRestaurant.RestaurantId);
        Console.WriteLine($"Restaurant deleted: ID {newRestaurant.RestaurantId}");

        await tableService.Delete(newTable.TableId);
        Console.WriteLine($"Table deleted: ID {newTable.TableId}");

        Console.WriteLine();
        Console.WriteLine("Demo completed. All create, update, and delete operations have been executed successfully.");
    }
}