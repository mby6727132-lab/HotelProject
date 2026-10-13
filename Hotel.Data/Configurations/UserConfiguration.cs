using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotel.Core.Entities;

namespace Hotel.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        // User.Email - Unique Index
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);

        // User-Payment - One-to-Many
        builder.HasMany(u => u.Payments)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // The User-Hotel Many-to-Many join table is configured once,
        // in HotelConfiguration, to avoid configuring it twice.

        // Seed data: 1 Admin + 2 Customers, so the system can be exercised immediately.
        // PasswordHash below is a real PBKDF2 hash (same algorithm as PasswordHasher) of "Passw0rd!".
        const string demoPasswordHash = "100000.mF3p0hT9c2vQd8bRzL1yAw==.TiLc1SeDoXndtQJguBGGhZ4m/lGYX+vEELXU9TttdjE=";
        builder.HasData(
            new User { Id = 1, Name = "Admin User", Email = "admin@hotelproject.com", PasswordHash = demoPasswordHash, Role = "Admin" },
            new User { Id = 2, Name = "Demo Customer", Email = "customer@hotelproject.com", PasswordHash = demoPasswordHash, Role = "Customer" },
            new User { Id = 3, Name = "Demo Customer Two", Email = "customer2@hotelproject.com", PasswordHash = demoPasswordHash, Role = "Customer" });
    }
}
