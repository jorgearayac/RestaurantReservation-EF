using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Services
{
    public class CustomerService
    {
        private readonly RestaurantReservationDbContext _context;

        public CustomerService(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> Update(Customer updatedCustomer)
        {
            var existing = await _context.Customers.FirstOrDefaultAsync(c =>
                            c.CustomerId == updatedCustomer.CustomerId);

            if (existing == null)
            {
                return false;
            }

            existing.FirstName = updatedCustomer.FirstName;
            existing.LastName = updatedCustomer.LastName;
            existing.Email = updatedCustomer.Email;
            existing.PhoneNumber = updatedCustomer.PhoneNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int customerId)
        {
            var existing = await _context.Customers.FindAsync(customerId);

            if (existing == null)
            {
                return false;
            }

            _context.Customers.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
