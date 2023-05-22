using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFishingRegulation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FishingRegulations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Regulation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishingRegulations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "81b20a47-088f-4de4-9142-cc3b30a78da9", "AQAAAAIAAYagAAAAELUmnIdC4k9Ew7dcS5BADqldkZoPhET5CJ0b0AW4sS/aVKyYXilLpHPZ9kqzXfC/qQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishingRegulations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "00c4cff6-f0dd-40f5-af02-ca6666cd5a32", "AQAAAAIAAYagAAAAEDxrK8PKkvp05innnDT96XcFS2tqsSMoXX7zTCVx4dqp4OntVOq+nD2LDGI8tzrk7A==" });
        }
    }
}
