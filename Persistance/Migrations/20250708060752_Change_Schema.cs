using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Change_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "SensorTypes",
                newName: "SensorTypes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Sensors",
                newName: "Sensors",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Lsn50V2Measurements",
                newName: "Lsn50V2Measurements",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Lsn50V2Lifecycles",
                newName: "Lsn50V2Lifecycles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FishTypes",
                newName: "FishTypes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FishingRegulations",
                newName: "FishingRegulations",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FishingLicenses",
                newName: "FishingLicenses",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FishingClubs",
                newName: "FishingClubs",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Catches",
                newName: "Catches",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "CatchDetails",
                newName: "CatchDetails",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Addresses",
                newSchema: "dbo");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a0e52d9f-f3bf-448a-9c9e-8d884e2001bf", "AQAAAAIAAYagAAAAEJ7STk73ch5ecbL3D2tcE7lzHw0GOJX7jRJHNBmqi4cx7IBpCTg3Ge29SqXJSax/kw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SensorTypes",
                schema: "dbo",
                newName: "SensorTypes");

            migrationBuilder.RenameTable(
                name: "Sensors",
                schema: "dbo",
                newName: "Sensors");

            migrationBuilder.RenameTable(
                name: "Lsn50V2Measurements",
                schema: "dbo",
                newName: "Lsn50V2Measurements");

            migrationBuilder.RenameTable(
                name: "Lsn50V2Lifecycles",
                schema: "dbo",
                newName: "Lsn50V2Lifecycles");

            migrationBuilder.RenameTable(
                name: "FishTypes",
                schema: "dbo",
                newName: "FishTypes");

            migrationBuilder.RenameTable(
                name: "FishingRegulations",
                schema: "dbo",
                newName: "FishingRegulations");

            migrationBuilder.RenameTable(
                name: "FishingLicenses",
                schema: "dbo",
                newName: "FishingLicenses");

            migrationBuilder.RenameTable(
                name: "FishingClubs",
                schema: "dbo",
                newName: "FishingClubs");

            migrationBuilder.RenameTable(
                name: "Catches",
                schema: "dbo",
                newName: "Catches");

            migrationBuilder.RenameTable(
                name: "CatchDetails",
                schema: "dbo",
                newName: "CatchDetails");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "dbo",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "dbo",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "dbo",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "dbo",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "dbo",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "dbo",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "dbo",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Addresses",
                schema: "dbo",
                newName: "Addresses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cab99e6a-a45a-4137-853c-a1247ad2ec95", "AQAAAAIAAYagAAAAEHtvvfJ/B6StevlbpLe2H9dLCyhzPmc/bmsdjuyxHnvwBV+B0t8ODpfOSDjDAK+FxA==" });
        }
    }
}
