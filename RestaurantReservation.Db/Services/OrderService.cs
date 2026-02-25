using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Services
{
    public class OrderService
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Create(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> Update(Order updatedOrder)
        {
            var existing = await _context.Orders.FirstOrDefaultAsync(o =>
                            o.OrderId == updatedOrder.OrderId);

            if (existing == null)
            {
                return false;
            }

            existing.OrderDate = updatedOrder.OrderDate;
            existing.TotalAmount = updatedOrder.TotalAmount;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int orderId)
        {
            var existing = await _context.Orders.FindAsync(orderId);

            if (existing == null)
            {
                return false;
            }

            _context.Orders.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Method to list all orders placed on a specific reservation, including the menu items ordered.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        public async Task<List<Order>> ListOrdersAndMenuItems(int reservationId)
        {
            return await _context.Orders
                .Where(o => o.ReservationId == reservationId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();
        }
    }
}
