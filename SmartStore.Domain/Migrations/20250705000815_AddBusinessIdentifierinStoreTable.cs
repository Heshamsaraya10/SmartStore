using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStore.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessIdentifierinStoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessType",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistrationNumber",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstablishmentDate",
                table: "Stores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalName",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCardNumber",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxRegistrationNumber",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessType",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CommercialRegistrationNumber",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "EstablishmentDate",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LegalName",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "TaxCardNumber",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "TaxRegistrationNumber",
                table: "Stores");
        }
    }
}
