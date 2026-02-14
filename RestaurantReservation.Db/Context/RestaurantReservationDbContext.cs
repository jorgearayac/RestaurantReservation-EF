using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Db.Context;

public class RestaurantDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = RestaurantReservationCore"
            );
    }
}
