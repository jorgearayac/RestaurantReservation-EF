using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories
{
    public class OrderItemRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public OrderItemRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> Create(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> Update(OrderItem updatedOrderItem)
        {
            var existing = await _context.OrderItems.FirstOrDefaultAsync(o =>
                            o.OrderItemId == updatedOrderItem.OrderItemId);

            if (existing == null)
            {
                return false;
            }

            existing.Quantity = updatedOrderItem.Quantity;
            existing.ItemId = updatedOrderItem.ItemId;
            existing.OrderId = updatedOrderItem.OrderId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int orderItemId)
        {
            var existing = await _context.OrderItems.FindAsync(orderItemId);

            if (existing == null)
            {
                return false;
            }

            _context.OrderItems.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
