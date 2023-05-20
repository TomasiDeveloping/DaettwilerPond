using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixSystemAdminSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash" },
                values: new object[] { "b4dd7b3e-36c1-4b75-8921-e8335e62e073", true, "AQAAAAIAAYagAAAAEEdvxyK457bAiUS7bRmy6dFhrDyY80rr3UPZ3Kn9mR/HDjGE+C8T/BkiMNQqY/uFKA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash" },
                values: new object[] { "c95c3382-dbf0-42db-88cd-ebf31b2a8d11", false, "AQAAAAIAAYagAAAAEF3zKwWm70mWjo1xjJ/HXPBMunTeZvqsOd7OUCWZw4N8bv6dK7jbw7qVhzP71cenBg==" });
        }
    }
}
