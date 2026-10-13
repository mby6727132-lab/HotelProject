using Hotel.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Data.Configurations;

public class InquiryConfiguration : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.ToTable("Inquiries");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.ResponseText)
            .HasMaxLength(2000);

        // User-Inquiry - One-to-Many
        builder.HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Hotel-Inquiry - One-to-Many
        builder.HasOne(i => i.Hotel)
            .WithMany()
            .HasForeignKey(i => i.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.UserId);
    }
}
