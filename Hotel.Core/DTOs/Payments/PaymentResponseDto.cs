namespace Hotel.Core.DTOs.Payments;

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public string GuestFirstName { get; set; } = string.Empty;
    public string GuestLastName { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
}
