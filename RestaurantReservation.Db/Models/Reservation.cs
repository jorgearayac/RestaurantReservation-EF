using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantReservation.Db.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }

        public Customer Customers { get; set; } = null!;
        public Restaurant Restaurants { get; set; } = null!;
        public Table Tables { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
