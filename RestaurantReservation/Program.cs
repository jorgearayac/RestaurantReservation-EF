// See https://aka.ms/new-console-template for more information
using RestaurantReservation.Db.Context;

using (var context = new RestaurantReservationDbContext())
{
    context.Database.EnsureCreated();
}