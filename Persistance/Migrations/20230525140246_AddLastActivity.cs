using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3939759c-f6a1-4a5b-a28f-f99e8dcca83e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f63c078a-dc82-4c07-a01b-0f0f0730882b"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ea2a36e6-401f-4d44-ac17-e19e05fa3211"), new Guid("1e59351a-d970-428d-b63e-a141578f0184") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ea2a36e6-401f-4d44-ac17-e19e05fa3211"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3939759c-f6a1-4a5b-a28f-f99e8dcca83e"), null, "User", "USER" },
                    { new Guid("ea2a36e6-401f-4d44-ac17-e19e05fa3211"), null, "Administrator", "ADMINISTRATOR" },
                    { new Guid("f63c078a-dc82-4c07-a01b-0f0f0730882b"), null, "Aufseher", "AUFSEHER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1e59351a-d970-428d-b63e-a141578f0184"), 0, "81b20a47-088f-4de4-9142-cc3b30a78da9", "info@tomasi-developing.ch", true, "System", true, "Administrator", false, null, "info@tomasi-developing.ch", "INFO@TOMASI-DEVELOPING.CH", "AQAAAAIAAYagAAAAELUmnIdC4k9Ew7dcS5BADqldkZoPhET5CJ0b0AW4sS/aVKyYXilLpHPZ9kqzXfC/qQ==", null, false, null, false, "info@tomasi-developing.ch" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("ea2a36e6-401f-4d44-ac17-e19e05fa3211"), new Guid("1e59351a-d970-428d-b63e-a141578f0184") });
        }
    }
}
