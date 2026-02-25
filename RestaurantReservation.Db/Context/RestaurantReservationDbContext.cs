using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Models.Views;
using static RestaurantReservation.Db.Models.Views.EmployeeRestaurantDetailsView;


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
    public DbSet<ReservationDetailsView> ReservationDetailsView { get; set; }
    public DbSet<EmployeeRestaurantDetailsView> EmployeeRestaurantDetailsView { get; set; }

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
                  .HasForeignKey(e => e.RestaurantId)
                  .OnDelete(DeleteBehavior.Restrict);
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
                  .HasForeignKey(e => e.RestaurantId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Tables configuration
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId);

            entity.Property(e => e.Capacity)
                  .IsRequired();

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.Tables)
                  .HasForeignKey(e => e.RestaurantId)
                  .OnDelete(DeleteBehavior.Restrict);
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
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Restaurant)
                  .WithMany(r => r.Reservations)
                  .HasForeignKey(e => e.RestaurantId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Table)
                  .WithMany(t => t.Reservations)
                  .HasForeignKey(e => e.TableId)
                  .OnDelete(DeleteBehavior.Restrict);
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
                  .HasForeignKey(e => e.ReservationId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Employee)
                  .WithMany(emp => emp.Orders)
                  .HasForeignKey(e => e.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItems configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.Property(e => e.Quantity)
                  .IsRequired();

            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MenuItem)
                  .WithMany(m => m.OrderItems)
                  .HasForeignKey(e => e.ItemId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data
        SeedData(modelBuilder);

        // Views
        // Reservation Details
        modelBuilder.Entity<ReservationDetailsView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("View_ReservationDetails");
        });

        // Employee Restaurant Details
        modelBuilder.Entity<EmployeeRestaurantDetailsView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("View_EmployeeRestaurantDetails");
        });

        // Database functions
        modelBuilder
            .HasDbFunction(() => CalculateRestaurantTotalRevenue(default));
    }
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john@email.com", PhoneNumber = "100000001" },
            new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com", PhoneNumber = "100000002" },
            new Customer { CustomerId = 3, FirstName = "Michael", LastName = "Brown", Email = "michael@email.com", PhoneNumber = "100000003" },
            new Customer { CustomerId = 4, FirstName = "Emily", LastName = "Clark", Email = "emily@email.com", PhoneNumber = "100000004" },
            new Customer { CustomerId = 5, FirstName = "David", LastName = "Wilson", Email = "david@email.com", PhoneNumber = "100000005" }
        );

        // Seed Restaurants
        modelBuilder.Entity<Restaurant>().HasData(
            new Restaurant { RestaurantId = 1, Name = "Central Bistro", Address = "Main St 1", PhoneNumber = "111111111", OpeningHours = "08:00-22:00" },
            new Restaurant { RestaurantId = 2, Name = "Ocean View", Address = "Sea Rd 12", PhoneNumber = "222222222", OpeningHours = "09:00-23:00" },
            new Restaurant { RestaurantId = 3, Name = "Mountain Grill", Address = "Hill St 5", PhoneNumber = "333333333", OpeningHours = "10:00-22:00" },
            new Restaurant { RestaurantId = 4, Name = "City Lights", Address = "Downtown 8", PhoneNumber = "444444444", OpeningHours = "07:00-21:00" },
            new Restaurant { RestaurantId = 5, Name = "Green Garden", Address = "Park Ave 3", PhoneNumber = "555555555", OpeningHours = "08:00-20:00" }
        );

        // Seed MenuItems
        modelBuilder.Entity<MenuItem>().HasData(
            new MenuItem { ItemId = 1, Name = "Pizza", Description = "Classic pizza", Price = 10.99m, RestaurantId = 1 },
            new MenuItem { ItemId = 2, Name = "Burger", Description = "Beef burger", Price = 8.50m, RestaurantId = 2 },
            new MenuItem { ItemId = 3, Name = "Pasta", Description = "Italian pasta", Price = 12.00m, RestaurantId = 3 },
            new MenuItem { ItemId = 4, Name = "Salad", Description = "Fresh salad", Price = 7.25m, RestaurantId = 4 },
            new MenuItem { ItemId = 5, Name = "Steak", Description = "Grilled steak", Price = 18.75m, RestaurantId = 5 }
        );

        // Seed Employees
        modelBuilder.Entity<Employee>().HasData(
            new Employee { EmployeeId = 1, FirstName = "Anna", LastName = "White", Position = "Manager", RestaurantId = 1 },
            new Employee { EmployeeId = 2, FirstName = "Mark", LastName = "Taylor", Position = "Chef", RestaurantId = 2 },
            new Employee { EmployeeId = 3, FirstName = "Luke", LastName = "Harris", Position = "Waiter", RestaurantId = 3 },
            new Employee { EmployeeId = 4, FirstName = "Sophie", LastName = "Martin", Position = "Waiter", RestaurantId = 4 },
            new Employee { EmployeeId = 5, FirstName = "Chris", LastName = "Lee", Position = "Chef", RestaurantId = 5 }
        );

        // Seed Tables
        modelBuilder.Entity<Table>().HasData(
            new Table { TableId = 1, Capacity = 2, RestaurantId = 1 },
            new Table { TableId = 2, Capacity = 4, RestaurantId = 2 },
            new Table { TableId = 3, Capacity = 6, RestaurantId = 3 },
            new Table { TableId = 4, Capacity = 4, RestaurantId = 4 },
            new Table { TableId = 5, Capacity = 8, RestaurantId = 5 }
        );

        // Seed Reservations
        modelBuilder.Entity<Reservation>().HasData(
            new Reservation { ReservationId = 1, ReservationDate = new DateTime(2026, 1, 10), PartySize = 2, CustomerId = 1, RestaurantId = 1, TableId = 1 },
            new Reservation { ReservationId = 2, ReservationDate = new DateTime(2026, 1, 11), PartySize = 4, CustomerId = 2, RestaurantId = 2, TableId = 2 },
            new Reservation { ReservationId = 3, ReservationDate = new DateTime(2026, 1, 12), PartySize = 3, CustomerId = 3, RestaurantId = 3, TableId = 3 },
            new Reservation { ReservationId = 4, ReservationDate = new DateTime(2026, 1, 13), PartySize = 5, CustomerId = 4, RestaurantId = 4, TableId = 4 },
            new Reservation { ReservationId = 5, ReservationDate = new DateTime(2026, 1, 14), PartySize = 6, CustomerId = 5, RestaurantId = 5, TableId = 5 }
        );

        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderId = 1, OrderDate = new DateTime(2026, 1, 10), TotalAmount = 25.99m, ReservationId = 1, EmployeeId = 1 },
            new Order { OrderId = 2, OrderDate = new DateTime(2026, 1, 11), TotalAmount = 40.00m, ReservationId = 2, EmployeeId = 2 },
            new Order { OrderId = 3, OrderDate = new DateTime(2026, 1, 12), TotalAmount = 33.50m, ReservationId = 3, EmployeeId = 3 },
            new Order { OrderId = 4, OrderDate = new DateTime(2026, 1, 13), TotalAmount = 50.75m, ReservationId = 4, EmployeeId = 4 },
            new Order { OrderId = 5, OrderDate = new DateTime(2026, 1, 14), TotalAmount = 70.20m, ReservationId = 5, EmployeeId = 5 }
        );

        // Seed OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderItemId = 1, Quantity = 2, OrderId = 1, ItemId = 1 },
            new OrderItem { OrderItemId = 2, Quantity = 1, OrderId = 2, ItemId = 2 },
            new OrderItem { OrderItemId = 3, Quantity = 3, OrderId = 3, ItemId = 3 },
            new OrderItem { OrderItemId = 4, Quantity = 2, OrderId = 4, ItemId = 4 },
            new OrderItem { OrderItemId = 5, Quantity = 4, OrderId = 5, ItemId = 5 }
        );
    }

    /// <summary>
    /// Database function to calculate the total revenue from a specific restaurant.
    /// </summary>
    /// <param name="restaurantId"></param>
    /// <returns></returns>
    public async Task<decimal> CalculateRestaurantTotalRevenue(int restaurantId)
    {
        return await Database
            .SqlQueryRaw<decimal>(
                "SELECT dbo.fn_CalculateRestaurantRevenue(@restaurantId)",
                new SqlParameter("@restaurantId", restaurantId))
            .FirstAsync();
    }
}
