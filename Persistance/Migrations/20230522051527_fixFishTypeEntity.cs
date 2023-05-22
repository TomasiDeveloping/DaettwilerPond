using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixFishTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedSeasonFrom",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "ClosedSeasonTo",
                table: "FishTypes");

            migrationBuilder.AddColumn<int>(
                name: "ClosedSeasonFromDay",
                table: "FishTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedSeasonFromMonth",
                table: "FishTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedSeasonToDay",
                table: "FishTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedSeasonToMonth",
                table: "FishTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasClosedSeason",
                table: "FishTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasMinimumSize",
                table: "FishTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "00c4cff6-f0dd-40f5-af02-ca6666cd5a32", "AQAAAAIAAYagAAAAEDxrK8PKkvp05innnDT96XcFS2tqsSMoXX7zTCVx4dqp4OntVOq+nD2LDGI8tzrk7A==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedSeasonFromDay",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "ClosedSeasonFromMonth",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "ClosedSeasonToDay",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "ClosedSeasonToMonth",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "HasClosedSeason",
                table: "FishTypes");

            migrationBuilder.DropColumn(
                name: "HasMinimumSize",
                table: "FishTypes");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedSeasonFrom",
                table: "FishTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedSeasonTo",
                table: "FishTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1e59351a-d970-428d-b63e-a141578f0184"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6edbb22f-08bf-45e8-b2e9-3e20ae37a972", "AQAAAAIAAYagAAAAEC4PhWBd3ooCCFRxi9zTpZY89EJuPqT6J1MU2KtmOCsV4L8cpqV+eFF6lA/BpvYZnQ==" });
        }
    }
}
