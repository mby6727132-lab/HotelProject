using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Inquiries;

public class CreateInquiryDto
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    [MinLength(1)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Message { get; set; } = string.Empty;
}
