namespace Hotel.Core.DTOs.Hotels;

public class HotelResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string Amenities { get; set; } = string.Empty;
    public string? TargetAudience { get; set; }
    public decimal Rating { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
