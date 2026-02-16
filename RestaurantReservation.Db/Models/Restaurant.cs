using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantReservation.Db.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string OpeningHours { get; set; } = null!;

        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
