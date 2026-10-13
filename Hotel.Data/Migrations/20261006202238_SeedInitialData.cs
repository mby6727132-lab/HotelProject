using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "City", "Name" },
                values: new object[,]
                {
                    { 1, "99 HaYarkon St.", "Tel Aviv", "Dan Tel Aviv" },
                    { 2, "23 King David St.", "Jerusalem", "King David" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "admin@hotelproject.com", "Admin User", "100000.mF3p0hT9c2vQd8bRzL1yAw==.TiLc1SeDoXndtQJguBGGhZ4m/lGYX+vEELXU9TttdjE=", "Admin" },
                    { 2, "customer@hotelproject.com", "Demo Customer", "100000.mF3p0hT9c2vQd8bRzL1yAw==.TiLc1SeDoXndtQJguBGGhZ4m/lGYX+vEELXU9TttdjE=", "Customer" },
                    { 3, "customer2@hotelproject.com", "Demo Customer Two", "100000.mF3p0hT9c2vQd8bRzL1yAw==.TiLc1SeDoXndtQJguBGGhZ4m/lGYX+vEELXU9TttdjE=", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Floor", "HotelId", "LastBookingAt", "Price", "RoomNumber" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 450.00m, 101 },
                    { 2, 1, 1, null, 450.00m, 102 },
                    { 3, 2, 1, null, 620.00m, 201 },
                    { 4, 1, 2, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 700.00m, 101 },
                    { 5, 1, 2, null, 700.00m, 102 }
                });

            migrationBuilder.InsertData(
                table: "UserFavoriteHotels",
                columns: new[] { "HotelId", "UserId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "CheckIn", "CheckOut", "IsPaid", "RoomId", "UserId" },
                values: new object[,]
                {
                    { 1, 1800.00m, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 2 },
                    { 2, 2800.00m, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserFavoriteHotels",
                keyColumns: new[] { "HotelId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserFavoriteHotels",
                keyColumns: new[] { "HotelId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
