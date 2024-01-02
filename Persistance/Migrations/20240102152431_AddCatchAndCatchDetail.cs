using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCatchAndCatchDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IssuedBy",
                table: "FishingLicenses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Catches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoursSpent = table.Column<double>(type: "float", nullable: false),
                    FishingLicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catches_FishingLicenses_FishingLicenseId",
                        column: x => x.FishingLicenseId,
                        principalTable: "FishingLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatchDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FishTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    HadCrabs = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatchDetails_Catches_CatchId",
                        column: x => x.CatchId,
                        principalTable: "Catches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatchDetails_FishTypes_FishTypeId",
                        column: x => x.FishTypeId,
                        principalTable: "FishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "178c4669-cf5f-46a3-bae1-70deddfad8cd", "AQAAAAIAAYagAAAAEPQrZb+TqPepL+gVHF+aV0x0PQdT+8dI+KALi8YOBZ0eA/KuUotcf/+fOO/C153HaQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_CatchDetails_CatchId",
                table: "CatchDetails",
                column: "CatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CatchDetails_FishTypeId",
                table: "CatchDetails",
                column: "FishTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catches_FishingLicenseId",
                table: "Catches",
                column: "FishingLicenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatchDetails");

            migrationBuilder.DropTable(
                name: "Catches");

            migrationBuilder.AlterColumn<string>(
                name: "IssuedBy",
                table: "FishingLicenses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "727cf8e1-e5ef-418b-95cd-52d3a3c8e0b6", "AQAAAAIAAYagAAAAELQY7G3ZXHcHRUIjb8RAq929Z/CzYFZOMFZUquZbeeDHkT3sYO4s0JZDJBzY4Itofw==" });
        }
    }
}
