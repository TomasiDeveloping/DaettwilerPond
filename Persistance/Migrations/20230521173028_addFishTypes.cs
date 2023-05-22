using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFishTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ClosedSeasonFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedSeasonTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinimumSize = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6edbb22f-08bf-45e8-b2e9-3e20ae37a972", "AQAAAAIAAYagAAAAEC4PhWBd3ooCCFRxi9zTpZY89EJuPqT6J1MU2KtmOCsV4L8cpqV+eFF6lA/BpvYZnQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishTypes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4dd7b3e-36c1-4b75-8921-e8335e62e073", "AQAAAAIAAYagAAAAEEdvxyK457bAiUS7bRmy6dFhrDyY80rr3UPZ3Kn9mR/HDjGE+C8T/BkiMNQqY/uFKA==" });
        }
    }
}
