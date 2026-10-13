using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Rooms;

public class AvailableRoomsQueryDto
{
    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    // Optional: when provided, only rooms with Capacity >= Guests are returned.
    [Range(1, 50)]
    public int? Guests { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}
