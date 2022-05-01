using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    public partial class add_roles_with_authorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "NameOfRole" },
                values: new object[] { 1, "Manager" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "NameOfRole" },
                values: new object[] { 2, "Reception" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "NameOfRole" },
                values: new object[] { 3, "Techncian" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "NameOfRole" },
                values: new object[] { 4, "Cleaner" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
