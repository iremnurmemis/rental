using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class cardol2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals");

            migrationBuilder.AddForeignKey(
                name: "FK_CarRentals_Cards_CardId",
                table: "CarRentals",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }
    }
}
