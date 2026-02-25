using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class GetCustomersWithGreatPartySizeSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE PROCEDURE sp_GetCustomersWithGreatPartySize
                    @Guests INT
                AS
                BEGIN
                    SELECT DISTINCT 
                        c.CustomerId,
                        c.FirstName,
                        c.LastName,
                        c.Email
                    FROM Customers c
                    INNER JOIN Reservations r ON c.CustomerId = r.CustomerId
                    WHERE r.PartySize > @Guests
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_GetCustomersWithLargeReservations");
        }
    }
}
