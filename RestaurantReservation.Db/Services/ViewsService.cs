using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Views;

namespace RestaurantReservation.Db.Services
{
    public class ViewsService
    {
        private readonly RestaurantReservationDbContext _context;

        public ViewsService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method to retrieve all the reservations with their associated customer and restaurant details.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReservationDetailsView>> GetReservationDetails()
        {
            return await _context.ReservationDetailsView
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Method to retrieve employees with their associated restaurant details.
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeRestaurantDetailsView>> GetEmployeeRestaurantDetails()
        {
            return await _context.EmployeeRestaurantDetailsView
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
