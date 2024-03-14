using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "ImageUrl", "PasswordHash" },
                values: new object[] { "7132d3f5-425a-4e5a-847a-6af4d5299d37", null, "AQAAAAIAAYagAAAAECzN6Y/9f2JDqRvGeCdYe9Y5UjAxP0dn6T/68j5K9tiL8ypGwRLxKPfkhPQzUpR8LQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "24ab0d0a-8368-41ed-b4ee-7d12dbaa6f7b", "AQAAAAIAAYagAAAAEAuuMHoBB7c1VD2cA0PldM2i3JEIT7pWorJtkO9v7wz7VTZGXrWMJP/7fl1m3so45w==" });
        }
    }
}
