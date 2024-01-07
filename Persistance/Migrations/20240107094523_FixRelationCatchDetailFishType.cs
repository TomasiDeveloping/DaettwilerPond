using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationCatchDetailFishType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatchDetails_FishTypes_FishTypeId",
                table: "CatchDetails");

            migrationBuilder.DropIndex(
                name: "IX_CatchDetails_FishTypeId",
                table: "CatchDetails");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "64c7c191-b579-4f94-883f-077eccf2c7f6", "AQAAAAIAAYagAAAAEIaGl6/X6tg7uAJlgln6zyOcBoEuHLj1Jb5Xu7/GSLRWK+V2+9ZXUdD8BMeKHTcYrA==" });

            migrationBuilder.CreateIndex(
                name: "IX_CatchDetails_FishTypeId",
                table: "CatchDetails",
                column: "FishTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatchDetails_FishTypes_FishTypeId",
                table: "CatchDetails",
                column: "FishTypeId",
                principalTable: "FishTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatchDetails_FishTypes_FishTypeId",
                table: "CatchDetails");

            migrationBuilder.DropIndex(
                name: "IX_CatchDetails_FishTypeId",
                table: "CatchDetails");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6b4e2e68-b07b-4242-b8f1-80e37c86d60d", "AQAAAAIAAYagAAAAEAE/pyDdAOwaUw4nAr0vTgig38fcRgA9TDuhZTOlSopezrvBohH4zHfLT92BDQLS/g==" });

            migrationBuilder.CreateIndex(
                name: "IX_CatchDetails_FishTypeId",
                table: "CatchDetails",
                column: "FishTypeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CatchDetails_FishTypes_FishTypeId",
                table: "CatchDetails",
                column: "FishTypeId",
                principalTable: "FishTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
