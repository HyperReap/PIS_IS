using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    public partial class add_Employees_Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "ContractDueDae", "Email", "FirstName", "Password", "RoleId", "SecondName" },
                values: new object[] { 1, null, "manager@manager.cz", "Petr", "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", 1, "Novák" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "ContractDueDae", "Email", "FirstName", "Password", "RoleId", "SecondName" },
                values: new object[] { 2, new DateTime(2022, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "recepce@recepce.cz", "Michal", "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", 2, "Bloudný" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "ContractDueDae", "Email", "FirstName", "Password", "RoleId", "SecondName" },
                values: new object[] { 3, null, "technik@technik.cz", "Oto", "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", 3, "Ladský" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "ContractDueDae", "Email", "FirstName", "Password", "RoleId", "SecondName" },
                values: new object[] { 4, new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "uklizecka@uklizecka.cz", "Alena", "$2a$11$qahanh6DohzXdzZxzRuYAe.Bf01JgZTqXPgDI/OfZfLSIueZI3LIW", 4, "Novotná" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
