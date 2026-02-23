using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Services;
using System.ComponentModel.DataAnnotations;

class Program
{
    static async Task Main(string[] args)
    {
        using var context = new RestaurantReservationDbContext();
        var customerService = new CustomerService(context);

        var newCustomer = new Customer { FirstName = "Async", LastName = "Await", Email = "async@test.cl", PhoneNumber = "555555555" };
        await customerService.Create(newCustomer);
        Console.WriteLine($"Customer created: {newCustomer.FirstName} {newCustomer.LastName} (ID: {newCustomer.CustomerId})");
    }
}