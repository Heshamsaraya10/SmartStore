using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenTimeAndCloseTimeInStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "CloseTime",
                table: "Stores",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenTime",
                table: "Stores",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "Stores");
        }
    }
}
