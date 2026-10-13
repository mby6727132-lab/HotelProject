namespace Hotel.Core.Exceptions;

public class RoomUnavailableException : Exception
{
    public RoomUnavailableException(string message) : base(message) { }
}
