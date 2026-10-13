namespace Hotel.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // "Customer" or "Admin" (see Enums.UserRole)
    public string Role { get; set; } = "Customer";

    // User 1:N Payment
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // User N:M Hotel (favorites)
    public ICollection<Hotel> FavoriteHotels { get; set; } = new List<Hotel>();
}
