using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories
{
    public class TableRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public TableRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Table> Create(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<bool> Update(Table updatedTable)
        {
            var existing = await _context.Tables.FirstOrDefaultAsync(t =>
                            t.TableId == updatedTable.TableId);

            if (existing == null)
            {
                return false;
            }

            existing.Capacity = updatedTable.Capacity;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int tableId)
        {
            var existing = await _context.Tables.FindAsync(tableId);

            if (existing == null)
            {
                return false;
            }

            _context.Tables.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}