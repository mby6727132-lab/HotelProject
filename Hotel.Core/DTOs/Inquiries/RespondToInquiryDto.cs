using System.ComponentModel.DataAnnotations;

namespace Hotel.Core.DTOs.Inquiries;

public class RespondToInquiryDto
{
    [Required]
    [MinLength(1)]
    public string ResponseText { get; set; } = string.Empty;
}
