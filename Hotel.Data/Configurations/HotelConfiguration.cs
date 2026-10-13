using HotelEntity = Hotel.Core.Entities.Hotel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotel.Core.Entities;

namespace Hotel.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<HotelEntity>
{
    public void Configure(EntityTypeBuilder<HotelEntity> builder)
    {
        builder.ToTable("Hotels");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(h => h.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(h => h.Style)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(h => h.Amenities)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(h => h.TargetAudience)
            .HasMaxLength(200);

        builder.Property(h => h.Rating)
            .IsRequired()
            .HasColumnType("decimal(2,1)");

        // Hotel.City - Index
        builder.HasIndex(h => h.City)
            .HasDatabaseName("IX_Hotels_City");

        // Hotel-Room - One-to-Many
        builder.HasMany(h => h.Rooms)
            .WithOne(r => r.Hotel)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Hotel-HotelImage - One-to-Many
        builder.HasMany(h => h.Images)
            .WithOne(i => i.Hotel)
            .HasForeignKey(i => i.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // User-Hotel - Many-to-Many (FavoriteHotels / FavoritedByUsers)
        builder.HasMany(h => h.FavoritedByUsers)
            .WithMany(u => u.FavoriteHotels)
            .UsingEntity<Dictionary<string, object>>(
                "UserFavoriteHotels",
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<HotelEntity>().WithMany().HasForeignKey("HotelId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("UserId", "HotelId");
                    j.ToTable("UserFavoriteHotels");
                    // Seed: Customer (Id=2) favorites Dan Tel Aviv, Customer (Id=3) favorites King David
                    j.HasData(
                        new { UserId = 2, HotelId = 1 },
                        new { UserId = 3, HotelId = 2 });
                });

        // Seed data: lets the system be exercised immediately after database setup
        builder.HasData(
            new HotelEntity
            {
                Id = 1,
                Name = "Dan Tel Aviv",
                City = "Tel Aviv",
                Address = "99 HaYarkon St.",
                Description = "Perched directly on Tel Aviv's golden beachfront, Dan Tel Aviv blends vibrant city energy with laid-back Mediterranean charm. Steps from the promenade, trendy cafes, and the bustling Carmel Market, it's the perfect base for exploring Israel's most exciting city.",
                Style = "Vibrant & Contemporary",
                Amenities = "Free WiFi,Beachfront Pool,Rooftop Bar,Fitness Center,24/7 Room Service,Valet Parking",
                TargetAudience = "Couples, Business Travelers & City Explorers",
                Rating = 4.6m
            },
            new HotelEntity
            {
                Id = 2,
                Name = "King David",
                City = "Jerusalem",
                Address = "23 King David St.",
                Description = "A legendary address since 1931, the King David has hosted royalty, presidents, and dignitaries from around the world. Overlooking the Old City walls, it offers timeless elegance in the heart of Jerusalem.",
                Style = "Historic & Elegant",
                Amenities = "Free WiFi,Outdoor Pool,Spa & Wellness Center,Fine Dining Restaurant,Concierge Service,Garden Terrace",
                TargetAudience = "Luxury Travelers, Couples & History Enthusiasts",
                Rating = 4.8m
            },
            new HotelEntity
            {
                Id = 3,
                Name = "Carmel View Haifa",
                City = "Haifa",
                Address = "5 Yefe Nof St.",
                Description = "Set on the slopes of Mount Carmel overlooking the Bahai Gardens and the Mediterranean coastline, Carmel View offers a relaxed escape with some of the most spectacular views in Israel.",
                Style = "Scenic & Relaxed",
                Amenities = "Free WiFi,Panoramic Pool,Free Parking,Breakfast Included,Pet Friendly,Garden Lounge",
                TargetAudience = "Families, Nature Lovers & Relaxation Seekers",
                Rating = 4.3m
            });
    }
}
