using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.Entities;

public class Room
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public decimal Price { get; set; }

    // Marketing/detail content for the room-type listing
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    // Simple comma-separated list (e.g. "King Bed,Sea View,Air Conditioning")
    public string Amenities { get; set; } = string.Empty;

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;

    // Room 1:N RoomImage
    public ICollection<RoomImage> Images { get; set; } = new List<RoomImage>();

    public DateTime? LastBookingAt { get; set; }

    // Used for Optimistic Concurrency Control
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
