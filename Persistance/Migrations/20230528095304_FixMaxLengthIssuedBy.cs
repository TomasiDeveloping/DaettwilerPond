using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixMaxLengthIssuedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "99a92c04-2a31-4ad4-8b11-410ced6a5d05", "AQAAAAIAAYagAAAAELbx+ha4zGy5W3xqLDQAzggRT3Z7U67UAxktLEwhrh7CLsJh1LKY07VA8oiEc3fGcA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c0ffefa7-9926-4336-8c46-5559f240296b", "AQAAAAIAAYagAAAAEL25kuTvb7G5+0Lf5GcAMyMxEFfDCKxRn3cKElY3grm81+iw52kgMXdK12Zhx2AoEA==" });
        }
    }
}
