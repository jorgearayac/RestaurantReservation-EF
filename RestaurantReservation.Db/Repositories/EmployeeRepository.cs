using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Context;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories
{
    public class EmployeeRepository
    {
        private readonly RestaurantReservationDbContext _context;

        public EmployeeRepository(RestaurantReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> Update(Employee updatedEmployee)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e =>
                            e.EmployeeId == updatedEmployee.EmployeeId);
            
            if (existing == null)
            {
                return false;
            }

            existing.FirstName = updatedEmployee.FirstName;
            existing.LastName = updatedEmployee.LastName;
            existing.Position = updatedEmployee.Position;
            existing.RestaurantId = updatedEmployee.RestaurantId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int employeeId)
        {
            var existing = await _context.Employees.FindAsync(employeeId);

            if (existing == null)
            {
                return false;
            }

            _context.Employees.Remove(existing);

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all employees with the position of "Manager" from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> ListManagers()
        {
            return await _context.Employees
                .Where(e => e.Position == "Manager")
                .ToListAsync();
        }
    }
}
