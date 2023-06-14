using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFishingClub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishingClubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BillAddressName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BillAddress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    BillPostalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BillCity = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    IbanNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LicensePrice = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishingClubs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "727cf8e1-e5ef-418b-95cd-52d3a3c8e0b6", "AQAAAAIAAYagAAAAELQY7G3ZXHcHRUIjb8RAq929Z/CzYFZOMFZUquZbeeDHkT3sYO4s0JZDJBzY4Itofw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishingClubs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "99a92c04-2a31-4ad4-8b11-410ced6a5d05", "AQAAAAIAAYagAAAAELbx+ha4zGy5W3xqLDQAzggRT3Z7U67UAxktLEwhrh7CLsJh1LKY07VA8oiEc3fGcA==" });
        }
    }
}
