using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class carımagesforrental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarRentalId",
                table: "CarImages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "CarImages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_CarRentalId",
                table: "CarImages",
                column: "CarRentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_CarRentals_CarRentalId",
                table: "CarImages",
                column: "CarRentalId",
                principalTable: "CarRentals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_CarRentals_CarRentalId",
                table: "CarImages");

            migrationBuilder.DropIndex(
                name: "IX_CarImages_CarRentalId",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "CarRentalId",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "CarImages");
        }
    }
}
