namespace BookingSystem.Models;

public class Booking
{
    public int Id { get; set; }
    public string PassengerName { get; set; }
    public int FlightId { get; set; }
    public FlightClass Class { get; set; }
    public decimal Price { get; set; }
}