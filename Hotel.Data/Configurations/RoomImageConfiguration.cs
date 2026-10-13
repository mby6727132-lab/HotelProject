using Hotel.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations;

public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
{
    public void Configure(EntityTypeBuilder<RoomImage> builder)
    {
        builder.ToTable("RoomImages");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(i => i.DisplayOrder)
            .IsRequired();

        // Relationship (Room -> Images) is configured in RoomConfiguration to avoid configuring it twice.

        builder.HasData(BuildRoomImages());
    }

    private static RoomImage[] BuildRoomImages()
    {
        var images = new List<RoomImage>();
        var id = 1;
        for (var roomId = 1; roomId <= 12; roomId++)
        {
            images.Add(new RoomImage { Id = id++, RoomId = roomId, Url = $"https://picsum.photos/seed/room{roomId}-1/900/600", DisplayOrder = 1 });
            images.Add(new RoomImage { Id = id++, RoomId = roomId, Url = $"https://picsum.photos/seed/room{roomId}-2/900/600", DisplayOrder = 2 });
        }
        return images.ToArray();
    }
}
