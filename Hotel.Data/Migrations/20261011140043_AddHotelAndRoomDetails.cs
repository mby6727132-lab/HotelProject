using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAndRoomDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                table: "Rooms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rooms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestFirstName",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestLastName",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuestPhone",
                table: "Payments",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                table: "Hotels",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Hotels",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Hotels",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "Hotels",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HotelImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelImages_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomImages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HotelImages",
                columns: new[] { "Id", "DisplayOrder", "HotelId", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "https://picsum.photos/seed/hotel1-1/1000/667" },
                    { 2, 2, 1, "https://picsum.photos/seed/hotel1-2/1000/667" },
                    { 3, 3, 1, "https://picsum.photos/seed/hotel1-3/1000/667" },
                    { 4, 1, 2, "https://picsum.photos/seed/hotel2-1/1000/667" },
                    { 5, 2, 2, "https://picsum.photos/seed/hotel2-2/1000/667" },
                    { 6, 3, 2, "https://picsum.photos/seed/hotel2-3/1000/667" }
                });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amenities", "Description", "Style", "TargetAudience" },
                values: new object[] { "Free WiFi,Beachfront Pool,Rooftop Bar,Fitness Center,24/7 Room Service,Valet Parking", "Perched directly on Tel Aviv's golden beachfront, Dan Tel Aviv blends vibrant city energy with laid-back Mediterranean charm. Steps from the promenade, trendy cafes, and the bustling Carmel Market, it's the perfect base for exploring Israel's most exciting city.", "Vibrant & Contemporary", "Couples, Business Travelers & City Explorers" });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amenities", "Description", "Style", "TargetAudience" },
                values: new object[] { "Free WiFi,Outdoor Pool,Spa & Wellness Center,Fine Dining Restaurant,Concierge Service,Garden Terrace", "A legendary address since 1931, the King David has hosted royalty, presidents, and dignitaries from around the world. Overlooking the Old City walls, it offers timeless elegance in the heart of Jerusalem.", "Historic & Elegant", "Luxury Travelers, Couples & History Enthusiasts" });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "Amenities", "City", "Description", "Name", "Style", "TargetAudience" },
                values: new object[] { 3, "5 Yefe Nof St.", "Free WiFi,Panoramic Pool,Free Parking,Breakfast Included,Pet Friendly,Garden Lounge", "Haifa", "Set on the slopes of Mount Carmel overlooking the Bahai Gardens and the Mediterranean coastline, Carmel View offers a relaxed escape with some of the most spectacular views in Israel.", "Carmel View Haifa", "Scenic & Relaxed", "Families, Nature Lovers & Relaxation Seekers" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amount", "GuestFirstName", "GuestLastName", "GuestPhone" },
                values: new object[] { 1680.00m, "Demo", "Customer", "+972-50-1234567" });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amount", "GuestFirstName", "GuestLastName", "GuestPhone", "RoomId" },
                values: new object[] { 2400.00m, "Demo", "Customer Two", "+972-50-7654321", 5 });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "Id", "DisplayOrder", "RoomId", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "https://picsum.photos/seed/room1-1/900/600" },
                    { 2, 2, 1, "https://picsum.photos/seed/room1-2/900/600" },
                    { 3, 1, 2, "https://picsum.photos/seed/room2-1/900/600" },
                    { 4, 2, 2, "https://picsum.photos/seed/room2-2/900/600" },
                    { 5, 1, 3, "https://picsum.photos/seed/room3-1/900/600" },
                    { 6, 2, 3, "https://picsum.photos/seed/room3-2/900/600" },
                    { 7, 1, 4, "https://picsum.photos/seed/room4-1/900/600" },
                    { 8, 2, 4, "https://picsum.photos/seed/room4-2/900/600" },
                    { 9, 1, 5, "https://picsum.photos/seed/room5-1/900/600" },
                    { 10, 2, 5, "https://picsum.photos/seed/room5-2/900/600" }
                });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Amenities", "Capacity", "Description", "Name", "Price" },
                values: new object[] { "Queen Bed,Air Conditioning,Free WiFi,Smart TV", 2, "A cozy, light-filled room with a queen bed and city views - ideal for a comfortable, no-fuss stay.", "Standard Room", 420.00m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amenities", "Capacity", "Description", "Floor", "Name", "Price", "RoomNumber" },
                values: new object[] { "King Bed,Sea View,Minibar,Air Conditioning,Free WiFi,Smart TV", 2, "Spacious and stylish, with a king bed, a sitting area, and a partial sea view.", 3, "Deluxe Room", 620.00m, 301 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Amenities", "Capacity", "Description", "Name", "Price", "RoomNumber" },
                values: new object[] { "Two Queen Beds,Sofa Bed,Kitchenette,Air Conditioning,Free WiFi", 4, "Two connecting sleeping areas and extra space make this room perfect for families travelling together.", "Family Room", 850.00m, 205 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Amenities", "Capacity", "Description", "Floor", "HotelId", "LastBookingAt", "Name", "Price", "RoomNumber" },
                values: new object[] { "King Bed,Private Balcony,Sea View,Lounge Area,Premium Minibar,Free WiFi,Bathrobe & Slippers", 2, "Our most luxurious accommodation, with a separate living room, a private balcony, and panoramic sea views.", 6, 1, null, "Executive Suite", 1250.00m, 601 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Amenities", "Capacity", "Description", "Floor", "LastBookingAt", "Name", "Price" },
                values: new object[] { "Queen Bed,Air Conditioning,Free WiFi,Smart TV", 2, "Elegantly appointed with classic Jerusalem-stone accents and a comfortable queen bed.", 2, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Standard Room", 600.00m });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Amenities", "Capacity", "Description", "Floor", "HotelId", "LastBookingAt", "Name", "Price", "RoomNumber" },
                values: new object[,]
                {
                    { 6, "King Bed,Garden View,Minibar,Air Conditioning,Free WiFi", 2, "A refined room with a king bed and views over the hotel's manicured gardens.", 4, 2, null, "Deluxe Room", 850.00m, 404 },
                    { 7, "Two Queen Beds,Sofa Bed,Air Conditioning,Free WiFi,Smart TV", 4, "Generously sized with two sleeping areas, ideal for families visiting the Old City.", 3, 2, null, "Family Room", 1100.00m, 310 },
                    { 8, "King Bed,Old City View,Private Terrace,Butler Service,Premium Minibar,Bathrobe & Slippers", 2, "Our signature suite, featuring a private terrace with sweeping views of the Old City walls.", 5, 2, null, "Royal Suite", 1950.00m, 501 }
                });

            migrationBuilder.InsertData(
                table: "HotelImages",
                columns: new[] { "Id", "DisplayOrder", "HotelId", "Url" },
                values: new object[,]
                {
                    { 7, 1, 3, "https://picsum.photos/seed/hotel3-1/1000/667" },
                    { 8, 2, 3, "https://picsum.photos/seed/hotel3-2/1000/667" },
                    { 9, 3, 3, "https://picsum.photos/seed/hotel3-3/1000/667" }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "Id", "DisplayOrder", "RoomId", "Url" },
                values: new object[,]
                {
                    { 11, 1, 6, "https://picsum.photos/seed/room6-1/900/600" },
                    { 12, 2, 6, "https://picsum.photos/seed/room6-2/900/600" },
                    { 13, 1, 7, "https://picsum.photos/seed/room7-1/900/600" },
                    { 14, 2, 7, "https://picsum.photos/seed/room7-2/900/600" },
                    { 15, 1, 8, "https://picsum.photos/seed/room8-1/900/600" },
                    { 16, 2, 8, "https://picsum.photos/seed/room8-2/900/600" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Amenities", "Capacity", "Description", "Floor", "HotelId", "LastBookingAt", "Name", "Price", "RoomNumber" },
                values: new object[,]
                {
                    { 9, "Queen Bed,Air Conditioning,Free WiFi,Smart TV", 2, "A comfortable, well-lit room with partial garden views - great value for exploring Haifa.", 1, 3, null, "Standard Room", 320.00m, 103 },
                    { 10, "King Bed,Bay View,Private Balcony,Minibar,Free WiFi", 2, "Featuring a private balcony with sweeping views over the Bahai Gardens and the bay.", 3, 3, null, "Deluxe Room", 430.00m, 305 },
                    { 11, "Two Queen Beds,Single Bed,Air Conditioning,Free WiFi,Kitchenette", 5, "Our largest standard room, with flexible bedding for up to five guests.", 2, 3, null, "Family Room", 590.00m, 208 },
                    { 12, "King Bed,Panoramic View,Wraparound Balcony,Lounge Area,Premium Minibar,Free WiFi", 3, "A top-floor suite with a wraparound balcony overlooking the gardens and the sea beyond.", 5, 3, null, "Luxury Suite", 980.00m, 502 }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "Id", "DisplayOrder", "RoomId", "Url" },
                values: new object[,]
                {
                    { 17, 1, 9, "https://picsum.photos/seed/room9-1/900/600" },
                    { 18, 2, 9, "https://picsum.photos/seed/room9-2/900/600" },
                    { 19, 1, 10, "https://picsum.photos/seed/room10-1/900/600" },
                    { 20, 2, 10, "https://picsum.photos/seed/room10-2/900/600" },
                    { 21, 1, 11, "https://picsum.photos/seed/room11-1/900/600" },
                    { 22, 2, 11, "https://picsum.photos/seed/room11-2/900/600" },
                    { 23, 1, 12, "https://picsum.photos/seed/room12-1/900/600" },
                    { 24, 2, 12, "https://picsum.photos/seed/room12-2/900/600" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelImages_HotelId",
                table: "HotelImages",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomImages_RoomId",
                table: "RoomImages",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelImages");

            migrationBuilder.DropTable(
                name: "RoomImages");

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "GuestFirstName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GuestLastName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GuestPhone",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Amount",
                value: 1800.00m);

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Amount", "RoomId" },
                values: new object[] { 2800.00m, 4 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 450.00m);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Floor", "Price", "RoomNumber" },
                values: new object[] { 1, 450.00m, 102 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Price", "RoomNumber" },
                values: new object[] { 620.00m, 201 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Floor", "HotelId", "LastBookingAt", "Price", "RoomNumber" },
                values: new object[] { 1, 2, new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 700.00m, 101 });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Floor", "LastBookingAt", "Price" },
                values: new object[] { 1, null, 700.00m });
        }
    }
}
