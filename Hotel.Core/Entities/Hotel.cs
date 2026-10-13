namespace Hotel.Core.Entities;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    // Marketing/detail content for the listing and detail pages
    public string Description { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    // Simple comma-separated list (e.g. "Free WiFi,Pool,Spa,Parking")
    public string Amenities { get; set; } = string.Empty;
    public string? TargetAudience { get; set; }

    // 1-5 stars, one decimal place
    public decimal Rating { get; set; }

    // Hotel 1:N Room
    public ICollection<Room> Rooms { get; set; } = new List<Room>();

    // Hotel 1:N HotelImage
    public ICollection<HotelImage> Images { get; set; } = new List<HotelImage>();

    // Hotel N:M User (favorites)
    public ICollection<User> FavoritedByUsers { get; set; } = new List<User>();
}
