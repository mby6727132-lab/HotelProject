using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Hotels;

public class UpdateHotelDto
{
    [Required]
    [MinLength(2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Style { get; set; } = string.Empty;

    [Required]
    public string Amenities { get; set; } = string.Empty;

    public string? TargetAudience { get; set; }

    [Range(1, 5)]
    public decimal Rating { get; set; }
}
