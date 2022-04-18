using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    public partial class changing_dates_for_reservation_REPAIR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "RoomReservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "RoomReservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "RoomReservations");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "RoomReservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
