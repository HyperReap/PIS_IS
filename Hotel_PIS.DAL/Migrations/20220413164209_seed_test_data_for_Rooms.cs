using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_PIS.DAL.Migrations
{
    public partial class seed_test_data_for_Rooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Floor", "IsCleaned", "NumberOfBeds", "NumberOfSideBeds", "RoomNumber", "RoomSize" },
                values: new object[] { 1, 11, true, 111, 1111, 11111, 111111.0 });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Floor", "IsCleaned", "NumberOfBeds", "NumberOfSideBeds", "RoomNumber", "RoomSize" },
                values: new object[] { 2, 22, true, 222, 2222, 22222, 222222.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
