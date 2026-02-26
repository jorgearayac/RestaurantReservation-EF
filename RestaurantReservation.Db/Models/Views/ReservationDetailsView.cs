using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantReservation.Db.Models.Views
{
    public class ReservationDetailsView
    {
        public int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PartySize { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
    }
}
