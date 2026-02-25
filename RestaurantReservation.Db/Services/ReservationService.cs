using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Services
{
    public class ReservationService
    {
        private readonly RestaurantReservationDbContext _context;

        public ReservationService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> Create(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<bool> Update(Reservation updatedReservation)
        {
            var existing = await _context.Reservations.FirstOrDefaultAsync(r =>
                            r.ReservationId == updatedReservation.ReservationId);

            if (existing == null)
            {
                return false;
            }

            existing.ReservationDate = updatedReservation.ReservationDate;
            existing.PartySize = updatedReservation.PartySize;
            existing.TableId = updatedReservation.TableId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int reservationId)
        {
            var existing = await _context.Reservations.FindAsync(reservationId);

            if (existing == null)
            {
                return false;
            }

            _context.Reservations.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Method to get all reservations for a specific customer.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>
        /// A list of reservations made by that particular customer.
        /// </returns>
        public async Task<List<Reservation>> GetReservationsByCustomer(int customerId)
        {
            return await _context.Reservations
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }
    }
}