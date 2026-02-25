using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Services
{
    public class MenuItemService
    {
        private readonly RestaurantReservationDbContext _context;

        public MenuItemService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItem> Create(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return menuItem;
        }

        public async Task<bool> Update(MenuItem updatedMenuItem)
        {
            var existing = await _context.MenuItems.FirstOrDefaultAsync(m =>
                            m.ItemId == updatedMenuItem.ItemId);

            if (existing == null)
            {
                return false;
            }

            existing.Name = updatedMenuItem.Name;
            existing.Description = updatedMenuItem.Description;
            existing.Price = updatedMenuItem.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int itemId)
        {
            var existing = await _context.MenuItems.FindAsync(itemId);

            if (existing == null)
            {
                return false;
            }

            _context.MenuItems.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}