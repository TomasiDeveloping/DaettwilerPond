using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStartAndEndDateForFishing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndFishing",
                table: "Catches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartFishing",
                table: "Catches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6b4e2e68-b07b-4242-b8f1-80e37c86d60d", "AQAAAAIAAYagAAAAEAE/pyDdAOwaUw4nAr0vTgig38fcRgA9TDuhZTOlSopezrvBohH4zHfLT92BDQLS/g==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndFishing",
                table: "Catches");

            migrationBuilder.DropColumn(
                name: "StartFishing",
                table: "Catches");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "178c4669-cf5f-46a3-bae1-70deddfad8cd", "AQAAAAIAAYagAAAAEPQrZb+TqPepL+gVHF+aV0x0PQdT+8dI+KALi8YOBZ0eA/KuUotcf/+fOO/C153HaQ==" });
        }
    }
}
