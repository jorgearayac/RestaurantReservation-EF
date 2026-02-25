using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeRestaurantDetailsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW View_EmployeeRestaurantDetails 
                AS
                SELECT 
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.Position,
                    rest.RestaurantId,
                    rest.Name AS RestaurantName,
                    rest.Address AS RestaurantAddress
                FROM Employees e
                INNER JOIN Restaurants rest ON e.RestaurantId = rest.RestaurantId
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_EmployeeRestaurantDetails");
        }
    }
}
