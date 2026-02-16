using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Context;

public class RestaurantReservationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = RestaurantReservationCore"
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customers configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.PhoneNumber)
                  .HasMaxLength(20);
        });

        // Restaurants configuration
        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Address)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.PhoneNumber)
                  .HasMaxLength(20);

            entity.Property(e => e.OpeningHours)
                  .HasMaxLength(100);
        });

        // MenuItems configuration
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Description)
                  .HasMaxLength(500);

            entity.Property(e => e.Price)
                  .HasColumnType("decimal(10,2)");

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.MenuItems)
                  .HasForeignKey(e => e.RestaurantId);
        });

        // Employees configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.Property(e => e.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Position)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.Employees)
                  .HasForeignKey(e => e.RestaurantId);
        });

        // Tables configuration
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId);

            entity.Property(e => e.Capacity)
                  .IsRequired();

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.Tables)
                  .HasForeignKey(e => e.RestaurantId);
        });

        // Reservations configuration
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId);

            entity.Property(e => e.ReservationDate)
                  .IsRequired();

            entity.Property(e => e.PartySize)
                  .IsRequired();

            entity.HasOne(e => e.Customer)
                  .WithMany(c => c.Reservations)
                  .HasForeignKey(e => e.CustomerId);

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.Reservations)
                  .HasForeignKey(e => e.RestaurantId);

            entity.HasOne(e => e.Table)
                  .WithMany(t => t.Reservations)
                  .HasForeignKey(e => e.TableId);
        });

        // Orders configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.Property(e => e.OrderDate)
                  .IsRequired();

            entity.Property(e => e.TotalAmount)
                  .HasColumnType("decimal(10,2)");

            entity.HasOne(e => e.Reservation)
                  .WithMany(r => r.Orders)
                  .HasForeignKey(e => e.ReservationId);

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.Orders)
                  .HasForeignKey(e => e.EmployeeId);
        });

        // OrderItems configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.Property(e => e.Quantity)
                  .IsRequired();

            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(e => e.OrderId);

            entity.HasOne(e => e.MenuItem)
                  .WithMany(m => m.OrderItems)
                  .HasForeignKey(e => e.ItemId);
        });
    }
}
