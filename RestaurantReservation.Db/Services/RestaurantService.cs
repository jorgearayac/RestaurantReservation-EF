using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Services
{
    public class RestaurantService
    {
        private readonly RestaurantReservationDbContext _context;

        public RestaurantService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> Create(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<bool> Update(Restaurant updatedRestaurant)
        {
            var existing = await _context.Restaurants.FirstOrDefaultAsync(r =>
                            r.RestaurantId == updatedRestaurant.RestaurantId);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedRestaurant.Name;
            existing.Address = updatedRestaurant.Address;
            existing.PhoneNumber = updatedRestaurant.PhoneNumber;
            existing.OpeningHours = updatedRestaurant.OpeningHours;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int restaurantId)
        {
            var existing = await _context.Restaurants.FindAsync(restaurantId);

            if (existing == null)
            {
                return false;
            }

            _context.Restaurants.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
