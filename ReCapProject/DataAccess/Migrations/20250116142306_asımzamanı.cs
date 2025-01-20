using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class asımzamanı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "overdueHours",
                table: "CarRentals");

            migrationBuilder.AddColumn<DateTime>(
                name: "overdueEndDate",
                table: "CarRentals",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "overdueEndDate",
                table: "CarRentals");

            migrationBuilder.AddColumn<double>(
                name: "overdueHours",
                table: "CarRentals",
                type: "double precision",
                nullable: true);
        }
    }
}
