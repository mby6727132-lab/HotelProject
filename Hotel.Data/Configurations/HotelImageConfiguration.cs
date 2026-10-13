using Hotel.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations;

public class HotelImageConfiguration : IEntityTypeConfiguration<HotelImage>
{
    public void Configure(EntityTypeBuilder<HotelImage> builder)
    {
        builder.ToTable("HotelImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.DisplayOrder)
            .IsRequired();

        // Relationship (Hotel -> Images) is configured in HotelConfiguration to avoid configuring it twice.

        builder.HasData(
            new HotelImage { Id = 1, HotelId = 1, Url = "https://picsum.photos/seed/hotel1-1/1000/667", DisplayOrder = 1 },
            new HotelImage { Id = 2, HotelId = 1, Url = "https://picsum.photos/seed/hotel1-2/1000/667", DisplayOrder = 2 },
            new HotelImage { Id = 3, HotelId = 1, Url = "https://picsum.photos/seed/hotel1-3/1000/667", DisplayOrder = 3 },
            new HotelImage { Id = 4, HotelId = 2, Url = "https://picsum.photos/seed/hotel2-1/1000/667", DisplayOrder = 1 },
            new HotelImage { Id = 5, HotelId = 2, Url = "https://picsum.photos/seed/hotel2-2/1000/667", DisplayOrder = 2 },
            new HotelImage { Id = 6, HotelId = 2, Url = "https://picsum.photos/seed/hotel2-3/1000/667", DisplayOrder = 3 },
            new HotelImage { Id = 7, HotelId = 3, Url = "https://picsum.photos/seed/hotel3-1/1000/667", DisplayOrder = 1 },
            new HotelImage { Id = 8, HotelId = 3, Url = "https://picsum.photos/seed/hotel3-2/1000/667", DisplayOrder = 2 },
            new HotelImage { Id = 9, HotelId = 3, Url = "https://picsum.photos/seed/hotel3-3/1000/667", DisplayOrder = 3 });
    }
}
