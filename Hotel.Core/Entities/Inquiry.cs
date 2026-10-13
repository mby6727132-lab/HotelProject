namespace Hotel.Core.Entities;

public class Inquiry
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;

    // Captured from the contact form - may differ from User.Name, stays editable there
    public string FullName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Null = awaiting response; set = responded
    public string? ResponseText { get; set; }
    public DateTime? RespondedAt { get; set; }
}
