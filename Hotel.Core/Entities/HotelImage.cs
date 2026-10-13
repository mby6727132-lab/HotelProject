namespace Hotel.Core.Entities;

public class HotelImage
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
