using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotel.Core.Entities;

namespace Hotel.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CheckIn)
            .IsRequired();

        builder.Property(p => p.CheckOut)
            .IsRequired();

        // Payment.Amount - decimal(10,2)
        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(p => p.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.GuestFirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.GuestLastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.GuestPhone)
            .IsRequired()
            .HasMaxLength(30);

        // User-Payment - One-to-Many (FK configured on the User side too; kept here for clarity)
        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Room-Payment - One-to-Many
        builder.HasOne(p => p.Room)
            .WithMany()
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed data: sample bookings, one past (already checked out) and one future
        // (overlaps upcoming dates, useful for exercising the availability filter).
        builder.HasData(
            new Payment
            {
                Id = 1, UserId = 2, RoomId = 1, CheckIn = new DateTime(2026, 6, 10), CheckOut = new DateTime(2026, 6, 14),
                Amount = 1680.00m, IsPaid = true,
                GuestFirstName = "Demo", GuestLastName = "Customer", GuestPhone = "+972-50-1234567"
            },
            new Payment
            {
                Id = 2, UserId = 3, RoomId = 5, CheckIn = new DateTime(2026, 11, 1), CheckOut = new DateTime(2026, 11, 5),
                Amount = 2400.00m, IsPaid = true,
                GuestFirstName = "Demo", GuestLastName = "Customer Two", GuestPhone = "+972-50-7654321"
            });
    }
}
