using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    public partial class adding_default_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "TV" });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "WiFi" });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "John Cena" });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "Cost", "NumberOfPeople", "Payed", "ReservationState" },
                values: new object[] { 1, 100m, 4, 50m, 0 });

            migrationBuilder.InsertData(
                table: "RoomEquipment",
                columns: new[] { "Id", "EquipmentId", "RoomId" },
                values: new object[] { 1, 1, 2 });

            migrationBuilder.InsertData(
                table: "RoomEquipment",
                columns: new[] { "Id", "EquipmentId", "RoomId" },
                values: new object[] { 2, 3, 2 });

            migrationBuilder.InsertData(
                table: "RoomReservations",
                columns: new[] { "Id", "DateFrom", "DateTo", "ReservationId", "RoomId" },
                values: new object[] { 3, new DateTime(2022, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
