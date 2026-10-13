using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Payments;

public class CreatePaymentDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Required]
    [MinLength(1)]
    public string GuestFirstName { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string GuestLastName { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string GuestPhone { get; set; } = string.Empty;
}
