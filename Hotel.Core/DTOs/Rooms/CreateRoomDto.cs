using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Rooms;

public class CreateRoomDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int RoomNumber { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Floor { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public int HotelId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    [Required]
    public string Amenities { get; set; } = string.Empty;
}
