using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class ReservationDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW View_ReservationDetails
                AS
                SELECT 
                    r.ReservationId,
                    r.ReservationDate,
                    r.PartySize,
                    c.CustomerId,
                    c.FirstName + ' ' + c.LastName AS CustomerName,
                    rest.RestaurantId,
                    rest.Name AS RestaurantName
                FROM Reservations r
                INNER JOIN Customers c ON r.CustomerId = c.CustomerId
                INNER JOIN Tables t ON r.TableId = t.TableId
                INNER JOIN Restaurants rest ON t.RestaurantId = rest.RestaurantId
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_ReservationDetails");
        }
    }
}
