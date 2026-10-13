namespace Hotel.Core.DTOs.Inquiries;

public class AdminInquiryResponseDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ResponseText { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
