namespace Hotel.Core.Entities;

public class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }

    // Guest contact details captured on the booking form (may differ from the account holder)
    public string GuestFirstName { get; set; } = string.Empty;
    public string GuestLastName { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
}
