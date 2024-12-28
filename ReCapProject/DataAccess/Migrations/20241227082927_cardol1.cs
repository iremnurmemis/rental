using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class cardol1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "CarRentals",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarRentals_CardId",
                table: "CarRentals",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals");

            migrationBuilder.DropIndex(
                name: "IX_CarRentals_CardId",
                table: "CarRentals");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "CarRentals");
        }
    }
}
