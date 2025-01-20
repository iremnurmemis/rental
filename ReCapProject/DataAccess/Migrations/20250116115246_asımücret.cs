using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class asımücret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "overdueHours",
                table: "CarRentals",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "totalOverdueFee",
                table: "CarRentals",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "overdueHours",
                table: "CarRentals");

            migrationBuilder.DropColumn(
                name: "totalOverdueFee",
                table: "CarRentals");
        }
    }
}
