using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotel.Core.Entities;

namespace Hotel.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoomNumber)
            .IsRequired();

        builder.Property(r => r.Floor)
            .IsRequired();

        // Room.Price - decimal(10,2)
        builder.Property(r => r.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(r => r.Capacity)
            .IsRequired();

        builder.Property(r => r.Amenities)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.LastBookingAt);

        // Room.RowVersion - IsRowVersion / Concurrency Token
        builder.Property(r => r.RowVersion)
            .IsRowVersion();

        // Hotel-Room - One-to-Many (FK configured on the Hotel side too; kept here for clarity)
        builder.HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Room-RoomImage - One-to-Many
        builder.HasMany(r => r.Images)
            .WithOne(i => i.Room)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data: lets the system be exercised immediately after database setup.
        // RowVersion is DB-generated (rowversion column) and excluded from the INSERT regardless.
        builder.HasData(
            // --- Hotel 1: Dan Tel Aviv ---
            new Room
            {
                Id = 1, HotelId = 1, RoomNumber = 101, Floor = 1, Price = 420.00m, Capacity = 2,
                Name = "Standard Room",
                Description = "A cozy, light-filled room with a queen bed and city views - ideal for a comfortable, no-fuss stay.",
                Amenities = "Queen Bed,Air Conditioning,Free WiFi,Work Desk",
                LastBookingAt = new DateTime(2026, 6, 10)
            },
            new Room
            {
                Id = 2, HotelId = 1, RoomNumber = 301, Floor = 3, Price = 620.00m, Capacity = 2,
                Name = "Deluxe Room",
                Description = "Spacious and stylish, with a king bed, a sitting area, and a partial sea view.",
                Amenities = "King Bed,Sea View,Minibar,Air Conditioning,Free WiFi,Work Desk"
            },
            new Room
            {
                Id = 3, HotelId = 1, RoomNumber = 205, Floor = 2, Price = 850.00m, Capacity = 4,
                Name = "Family Room",
                Description = "Two connecting sleeping areas and extra space make this room perfect for families travelling together.",
                Amenities = "Two Queen Beds,Sofa Bed,Kitchenette,Air Conditioning,Free WiFi"
            },
            new Room
            {
                Id = 4, HotelId = 1, RoomNumber = 601, Floor = 6, Price = 1250.00m, Capacity = 2,
                Name = "Executive Suite",
                Description = "Our most luxurious accommodation, with a separate living room, a private balcony, and panoramic sea views.",
                Amenities = "King Bed,Private Balcony,Sea View,Lounge Area,Premium Minibar,Free WiFi,Bathrobe & Slippers"
            },

            // --- Hotel 2: King David ---
            new Room
            {
                Id = 5, HotelId = 2, RoomNumber = 102, Floor = 2, Price = 600.00m, Capacity = 2,
                Name = "Standard Room",
                Description = "Elegantly appointed with classic Jerusalem-stone accents and a comfortable queen bed.",
                Amenities = "Queen Bed,Air Conditioning,Free WiFi,Work Desk",
                LastBookingAt = new DateTime(2026, 11, 1)
            },
            new Room
            {
                Id = 6, HotelId = 2, RoomNumber = 404, Floor = 4, Price = 850.00m, Capacity = 2,
                Name = "Deluxe Room",
                Description = "A refined room with a king bed and views over the hotel's manicured gardens.",
                Amenities = "King Bed,Garden View,Minibar,Air Conditioning,Free WiFi"
            },
            new Room
            {
                Id = 7, HotelId = 2, RoomNumber = 310, Floor = 3, Price = 1100.00m, Capacity = 4,
                Name = "Family Room",
                Description = "Generously sized with two sleeping areas, ideal for families visiting the Old City.",
                Amenities = "Two Queen Beds,Sofa Bed,Air Conditioning,Free WiFi,Work Desk"
            },
            new Room
            {
                Id = 8, HotelId = 2, RoomNumber = 501, Floor = 5, Price = 1950.00m, Capacity = 2,
                Name = "Royal Suite",
                Description = "Our signature suite, featuring a private terrace with sweeping views of the Old City walls.",
                Amenities = "King Bed,Old City View,Private Terrace,Butler Service,Premium Minibar,Bathrobe & Slippers"
            },

            // --- Hotel 3: Carmel View Haifa ---
            new Room
            {
                Id = 9, HotelId = 3, RoomNumber = 103, Floor = 1, Price = 320.00m, Capacity = 2,
                Name = "Standard Room",
                Description = "A comfortable, well-lit room with partial garden views - great value for exploring Haifa.",
                Amenities = "Queen Bed,Air Conditioning,Free WiFi,Work Desk"
            },
            new Room
            {
                Id = 10, HotelId = 3, RoomNumber = 305, Floor = 3, Price = 430.00m, Capacity = 2,
                Name = "Deluxe Room",
                Description = "Featuring a private balcony with sweeping views over the Bahai Gardens and the bay.",
                Amenities = "King Bed,Bay View,Private Balcony,Minibar,Free WiFi"
            },
            new Room
            {
                Id = 11, HotelId = 3, RoomNumber = 208, Floor = 2, Price = 590.00m, Capacity = 5,
                Name = "Family Room",
                Description = "Our largest standard room, with flexible bedding for up to five guests.",
                Amenities = "Two Queen Beds,Single Bed,Air Conditioning,Free WiFi,Kitchenette"
            },
            new Room
            {
                Id = 12, HotelId = 3, RoomNumber = 502, Floor = 5, Price = 980.00m, Capacity = 3,
                Name = "Luxury Suite",
                Description = "A top-floor suite with a wraparound balcony overlooking the gardens and the sea beyond.",
                Amenities = "King Bed,Panoramic View,Wraparound Balcony,Lounge Area,Premium Minibar,Free WiFi"
            });
    }
}
