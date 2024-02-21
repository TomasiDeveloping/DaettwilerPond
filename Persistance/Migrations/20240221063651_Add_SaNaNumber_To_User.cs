using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_SaNaNumber_To_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaNaNumber",
                table: "AspNetUsers",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SaNaNumber" },
                values: new object[] { "24ab0d0a-8368-41ed-b4ee-7d12dbaa6f7b", "AQAAAAIAAYagAAAAEAuuMHoBB7c1VD2cA0PldM2i3JEIT7pWorJtkO9v7wz7VTZGXrWMJP/7fl1m3so45w==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaNaNumber",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "64c7c191-b579-4f94-883f-077eccf2c7f6", "AQAAAAIAAYagAAAAEIaGl6/X6tg7uAJlgln6zyOcBoEuHLj1Jb5Xu7/GSLRWK+V2+9ZXUdD8BMeKHTcYrA==" });
        }
    }
}
