namespace Hotel.Core.DTOs.Rooms;

public class RoomResponseDto
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public decimal Price { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Amenities { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
}
